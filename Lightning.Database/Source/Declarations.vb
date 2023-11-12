Imports System.Collections.Concurrent

Namespace Lightning
    Public Partial Class Database(Of T As {DatabaseObject})
        Private BackingStore As Dictionary(Of String, T)
        Private ReadOnly BackingStoreLock As Object
        Private BackingIndex As Dictionary(Of String, IndexEntry)
        Private ReadOnly BaseContentPath As String
        Private ReadOnly IndexFile As String
        
        Private ReadOnly TypeOfDatabase As Type = GetType(Database(Of T))
    End Class
End Namespace
