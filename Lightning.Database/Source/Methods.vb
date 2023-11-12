Imports System.Collections.Concurrent
Imports System.Text

Namespace Lightning
    Public Partial Class Database(Of T As {DatabaseObject})
        Public Sub New(ContentRoot As String)
            BaseContentPath = ContentRoot
            BackingStore = New ConcurrentDictionary(Of String, T)
            BackingIndex = New Dictionary(Of String, IndexEntry)
            CountThreshold = 10000
            TimeThreshold = New TimeSpan(0,0,0,1,0)
            BackingStoreLock = New Object
            IndexFile = ContentRoot & "/db.index"
        End Sub
        
        Public Sub LoadAll()
            SyncLock BackingStoreLock
                If IO.Directory.Exists(BaseContentPath) = False OrElse IO.File.Exists(IndexFile) = False Then Return
                
                Dim Entries As String() = IO.File.ReadAllLines(IndexFile)
                BackingIndex.Clear() : BackingIndex = New Dictionary(Of String, IndexEntry)( Entries.Length)
                BackingStore.Clear() : BackingStore = New ConcurrentDictionary(Of String, T)(Entries.Length, Entries.Length)

                Dim ParserQueue As New ParallelWorkerEngine(Of ParseWorker)(Entries.Length)
                Dim Indexer As Int32 = 0 : For Each Entry In Entries
                    ParserQueue.Enqueue(New ParseWorker(Entry, BackingIndex, Indexer)) 
                    Indexer += 1
                Next : ParserQueue.Run() : ParserQueue.WaitForAll() : ParserQueue.Clear()
                
                Dim LoaderQueue As New ParallelWorkerEngine(Of LoadWorker)(Entries.Length)
                For Each Entry In BackingIndex
                    LoaderQueue.Enqueue(New LoadWorker(Entry.Value, BaseContentPath, BackingStore))
                Next : LoaderQueue.Run() : LoaderQueue.WaitForAll() : LoaderQueue.Clear()
                
            End SyncLock
        End Sub
        
        Public Sub SaveModified()
            SyncLock BackingStoreLock
                If IO.Directory.Exists(BaseContentPath) = False Then 
                    IO.Directory.CreateDirectory(BaseContentPath)
                End If
                
                Dim SaveQueue As New ParallelWorkerEngine(Of SaveWorker) : For Each Entry in BackingStore
                    If Entry.Value.Dirty = True Then SaveQueue.Enqueue(New SaveWorker(Entry.Value, BackingIndex(Entry.Key), BaseContentPath))
                Next : SaveQueue.Run() : SaveQueue.WaitForAll() : SaveQueue.Clear()
                IO.File.WriteAllLines(IndexFile, GetEntries())
            End SyncLock
        End Sub
        
        Public Function CompareAdd(Entry As T) As Boolean
            If BackingStore.ContainsKey(Entry.ID) = True Then
                Return False
            End If
            SyncLock BackingStoreLock
                If BackingStore.ContainsKey(Entry.ID) = True Then 
                    Return False
                End If
                
                Dim SubcontentPathBuilder As New StringBuilder : SubcontentPathBuilder.
                    Append(BaseContentPath).
                    Append("/"c).
                    Append(ToHex(Entry.ID))
                Dim SubcontentPath As String = SubcontentPathBuilder.ToString()
                
                Dim FilePathBuilder As New StringBuilder : FilePathBuilder.
                    Append(SubcontentPath).
                    Append(".db")
                Dim FilePath As String = FilePathBuilder.ToString()
                
                BackingStore.TryAdd(Entry.ID, Entry)
                BackingIndex.Add(Entry.ID, New IndexEntry(Entry.ID, FilePath, SubcontentPath, BackingIndex.Count))
                Return True
            End SyncLock
        End Function
        
        Private Function GetEntries As String()
            Dim EntryList As New List(Of String)(BackingIndex.Count)
            For Each IndexItem In BackingIndex.Values
                EntryList.Add(IndexItem.EntryHex)
            Next
            Return EntryList.ToArray()
        End Function
        
        Public Function CompareRemove(Entry As T) As Boolean
            Return CompareRemove(Entry.ID)
        End Function
        
        Public Function CompareRemove(ID As String) As Boolean
            If BackingStore.ContainsKey(ID) = False Then Return False
            SyncLock BackingStoreLock
                If BackingStore.ContainsKey(ID) = False Then Return False
                
                Dim FilePath As String = BackingIndex(ID).FilePath
                
                Dim SubcontentPath As String = BackingIndex(ID).SubcontentPath
                
                If IO.File.Exists(FilePath) = True Then IO.File.Delete(FilePath)
                If IO.Directory.Exists(SubcontentPath) = True Then IO.Directory.Delete(SubcontentPath, True)
                BackingStore.TryRemove(ID, Nothing)
                BackingIndex.Remove(ID)
                
                Return True
            End SyncLock
        End Function
        
        Public Function Contains(Entry As T) As Boolean
            Return Contains(Entry.ID)
        End Function
        
        Public Function Contains(ID As String) As Boolean
            SyncLock BackingStoreLock
                Return BackingStore.ContainsKey(ID)
            End SyncLock
        End Function
    End Class
End Namespace