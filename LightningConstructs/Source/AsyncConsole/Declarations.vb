Public Partial Class AsyncConsole
    Private Shared FlushGovernor As New Governor(20.0)
    Private Shared FlushQueue As New QueueStream(Of String)
    Private Shared CloseFlushThread As Boolean = False
    Private Shared FlushLock As New Object
    Private Shared BufferEngineExists As Boolean = False
End Class