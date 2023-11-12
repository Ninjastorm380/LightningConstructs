Namespace Lightning
    Public Partial Class Database(Of T As {DatabaseObject})
        Public Property CountThreshold As Int32
        Public Property TimeThreshold As Timespan
        Public ReadOnly Property ContentPath As String 
            Get 
                Return BaseContentPath
            End Get
        End Property
        
        Public Default ReadOnly Property Item(ByVal ID As String) As T
            Get 
                If BackingStore.ContainsKey(ID) = False Then Return Nothing
                SyncLock BackingStoreLock
                    If BackingStore.ContainsKey(ID) = False Then Return Nothing
                    Return BackingStore(ID)
                End SyncLock
            End Get
        End Property
        
        Public ReadOnly Property Items As T()
            Get 
                SyncLock BackingStoreLock
                    Return BackingStore.Values.ToArray()
                End SyncLock
            End Get
        End Property
    End Class
End Namespace
