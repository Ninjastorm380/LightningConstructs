    Public Partial Class Socket
        Private NetSocket As System.Net.Sockets.Socket = New System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
        Private IsListening As System.Boolean = False
        Private IsConnected As System.Boolean = False
        Private IsBaseDisposed As System.Boolean = False
        Private ListenerGovernor As Governor = New Governor(20)
        Private ReadLock As System.Object = New System.Object()
        Private WriteLock As System.Object = New System.Object()
    End Class
