    Public Partial Class Socket
        Private NetSocket As Net.Sockets.Socket = New Net.Sockets.Socket(Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Private IsListening As System.Boolean = False
        Private IsConnected As System.Boolean = False
        Private IsBaseDisposed As System.Boolean = False
        Private ReadOnly ListenerGovernor As Governor = New Governor(10)
        Private DataAvailable As Boolean = False
        Private DataNotAvailable As Boolean = False
        Private BaseClientBound As Boolean = False
        
        Private ReadOnly ReadLock As System.Object = New System.Object()
        Private ReadTimeoutStopwatch As Stopwatch = New Stopwatch()
        
        Private ReadOnly WriteLock As System.Object = New System.Object()
    End Class
