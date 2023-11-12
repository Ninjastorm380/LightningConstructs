Imports System.Collections.Concurrent
Imports System.Text

Namespace Lightning
    Public Partial Class Database(Of T As {DatabaseObject})
        Public MustInherit Class DatabaseObject
            Private BaseDirty As Boolean
            Protected BaseID As String
            Protected ReadOnly ThreadedAccessLock As Object
            Protected Friend Sub New(ID As String)
                BaseID = ID
                ThreadedAccessLock = New Object
                BaseDirty = True
            End Sub
            Friend Sub BaseSave(ContentPath As String, FilePath As String, SubcontentPath As String)
                If BaseDirty = False Then Return
                SyncLock ThreadedAccessLock
                    If BaseDirty = False Then Return
                    Save(ContentPath, FilePath, SubcontentPath)
                    BaseDirty = False
                End SyncLock
            End Sub
            Friend Sub BaseLoad(ContentPath As String, FilePath As String, SubcontentPath As String)
                If BaseDirty = False Then Return
                SyncLock ThreadedAccessLock
                    If BaseDirty = False Then Return
                    Load(ContentPath, FilePath, SubcontentPath)
                    BaseDirty = False
                End SyncLock
            End Sub
            
            Friend Sub BaseInit(ContentPath As String, FilePath As String, SubcontentPath As String)
                SyncLock ThreadedAccessLock
                    Init(ContentPath, FilePath, SubcontentPath)
                End SyncLock
            End Sub
            Protected MustOverride Sub Save(ContentPath As String, FilePath As String, SubcontentPath As String)
            Protected MustOverride Sub Load(ContentPath As String, FilePath As String, SubcontentPath As String)
            Protected MustOverride Sub Init(ContentPath As String, FilePath As String, SubcontentPath As String)
            Public ReadOnly Property ID As String
                Get 
                    SyncLock ThreadedAccessLock
                        Return BaseID
                    End SyncLock
                End Get
            End Property
            
            Public Property Dirty As Boolean
                Get 
                    SyncLock ThreadedAccessLock
                        Return BaseDirty
                    End SyncLock
                End Get
                Protected Set
                    SyncLock ThreadedAccessLock
                        BaseDirty = Value
                    End SyncLock
                End Set
            End Property
        End Class
        Private Class IndexEntry
            Public ReadOnly Property ID As String
            Public ReadOnly Property IDHex As String
            Public ReadOnly Property Index As Int32
            Public ReadOnly Property FilePath As String
            Public ReadOnly Property FilePathHex As String
            Public ReadOnly Property SubcontentPath As String
            Public ReadOnly Property SubcontentPathHex As String
            
            Public ReadOnly Property Entry As String
            Public ReadOnly Property EntryHex As String
            
            Private Const Delimiter As Char = "-"c
            
            Public Sub New(ID As String, FilePath As String, SubcontentPath As String, Index As Int32)
                Me.ID = ID
                Me.FilePath = FilePath
                Me.SubcontentPath = SubcontentPath
                Me.Index = Index
                IDHex = ToHex(ID)
                FilePathHex = ToHex(FilePath)
                SubcontentPathHex = ToHex(SubcontentPath)
                Dim EntryBuilder As New StringBuilder : EntryBuilder.
                    Append(FilePathHex).
                    Append(Delimiter).
                    Append(IDHex).
                    Append(Delimiter).
                    Append(SubcontentPathHex)
                Entry = EntryBuilder.ToString()
                EntryHex = ToHex(Entry)
            End Sub
            
            Public Sub New(ID As String, FilePath As String, SubcontentPath As String, Entry As String, IDHex As String, FilePathHex As String, SubcontentPathHex As String, EntryHex As String, Index As Int32)
                Me.ID = ID
                Me.FilePath = FilePath
                Me.SubcontentPath = SubcontentPath
                Me.IDHex = IDHex
                Me.FilePathHex = FilePathHex
                Me.SubcontentPathHex = SubcontentPathHex
                Me.Entry = Entry
                Me.EntryHex = EntryHex
                Me.Index = Index
            End Sub
            
            Public Shared Function FromHexString(Input As String, Index As Int32) As IndexEntry
                Dim Entry As String = FromHex(Input)
                Dim Data As String() = Entry.Split(Delimiter)
                Dim FilePathHex As String = Data(0)
                Dim IDHex As String = Data(1)
                Dim SubcontentPathHex As String = Data(2)
                Dim FilePath As String = FromHex(FilePathHex)
                Dim ID As String = FromHex(IDHex)
                Dim SubcontentPath As String = FromHex(SubcontentPathHex)
                Dim Result As New IndexEntry(ID, FilePath, SubcontentPath, Entry, IDHex, FilePathHex, SubcontentPathHex, Input, Index)
                Return Result
            End Function
        End Class
        Private Class ParseWorker : Inherits ParallelWorkerEngine(Of ParseWorker).ParallelWorker
            Private BaseCompleted As Boolean
            Private ReadOnly BaseInput As String
            Private BaseOutput As IndexEntry
            Private ReadOnly Target As Dictionary(Of String,IndexEntry)
            Private ReadOnly TargetIndex As Int32
            Public ReadOnly Property Input As String
                Get 
                    Return BaseInput
                End Get
            End Property
            
            Public ReadOnly Property Output As IndexEntry
                Get 
                    Return BaseOutput
                End Get
            End Property
            
            Public Overrides ReadOnly Property Completed As Boolean
                Get 
                    Return BaseCompleted
                End Get
            End Property
            
            Public Overrides Sub Run()
                BaseCompleted = False
                LambdaThread.Start(AddressOf Worker, "ParseWorker work thread")
            End Sub
            
            Public Sub New(Input As String, Target As Dictionary(Of String,IndexEntry), Index As Int32)
                BaseInput = Input
                Me.Target = Target
                TargetIndex = Index
                BaseCompleted = True
            End Sub
            
            Private Sub Worker()
                BaseOutput = IndexEntry.FromHexString(Input, TargetIndex)
                If TargetIndex > 0
                    If Target.Count <> TargetIndex
                        Do Until Target.Count = TargetIndex
                            Threading.Thread.Sleep(0)
                        Loop 
                    End If
                End If
                Target.Add(BaseOutput.ID, BaseOutput)
                BaseCompleted = True
            End Sub
        End Class
        Private Class LoadWorker : Inherits ParallelWorkerEngine(Of LoadWorker).ParallelWorker
            Private ReadOnly BaseInput As IndexEntry
            Private BaseOutput As T
            Private BaseCompleted As Boolean
            Private ReadOnly ContentClassType As Type
            Private ReadOnly BaseContentPath As String
            Private ReadOnly Target As Dictionary(Of String, T)

            Public ReadOnly Property Input As IndexEntry
                Get 
                    Return BaseInput
                End Get
            End Property

            Public ReadOnly Property Output As T
                Get 
                    Return BaseOutput
                End Get
            End Property

            Public Overrides ReadOnly Property Completed As Boolean
                Get 
                    Return BaseCompleted
                End Get
            End Property


            Public Sub New(Entry As IndexEntry, BaseContentPath As String, Target As Dictionary(Of String, T))
                BaseInput = Entry
                ContentClassType = GetType(T)
                Me.BaseContentPath = BaseContentPath
                Me.Target = Target
                BaseCompleted = True
            End Sub
            
            Public Overrides Sub Run()
                BaseCompleted = False
                LambdaThread.Start(AddressOf Worker, "LoadWorker work thread")
            End Sub
            
            Private Sub Worker()
                BaseOutput = CType(Activator.CreateInstance(ContentClassType, BaseInput.ID), T)
                BaseOutput.BaseInit(BaseContentPath, BaseInput.FilePath, BaseInput.SubcontentPath)
                BaseOutput.BaseLoad(BaseContentPath, BaseInput.FilePath, BaseInput.SubcontentPath)
                Target.Add(BaseOutput.ID, BaseOutput)
                BaseCompleted = True
            End Sub
        End Class
        Private Class SaveWorker : Inherits ParallelWorkerEngine(Of SaveWorker).ParallelWorker
            Private BaseInput As T
            Private BaseCompleted As Boolean
            Private BaseTriggered As Boolean
            Private ReadOnly BaseContentPath As String
            Private ReadOnly IndexEntry As IndexEntry
            Public ReadOnly Property Input As T
                Get 
                    Return BaseInput
                End Get
            End Property

            Public Overrides ReadOnly Property Completed As Boolean
                Get 
                    Return BaseCompleted
                End Get
            End Property
            
            Public Sub New(Entry As T, IndexEntry As IndexEntry, BaseContentPath As String)
                BaseInput = Entry
                Me.BaseContentPath = BaseContentPath
                Me.IndexEntry = IndexEntry
                BaseCompleted = True
            End Sub
            
            Public Overrides Sub Run()
                BaseCompleted = False
                BaseTriggered = True
                LambdaThread.Start(AddressOf Worker, "SaveWorker work thread")
            End Sub
            
            Private Sub Worker()
                BaseInput.BaseSave(BaseContentPath, IndexEntry.FilePath, IndexEntry.SubcontentPath)
                BaseCompleted = True
                'BaseTriggered = False
            End Sub
        End Class
    End Class
End Namespace
