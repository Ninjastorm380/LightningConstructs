    Public Partial Class Socket
        Private NetSocket As Net.Sockets.Socket = New Net.Sockets.Socket(Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Private IsListening As System.Boolean = False
        Private IsConnected As System.Boolean = False
        Private IsBaseDisposed As System.Boolean = False
        Private ReadOnly ListenerGovernor As Governor = New Governor(10)
        
        Private ReadOnly ReadLock As System.Object = New System.Object()
        Private ReadRetryResult As UInt32 = 0
        Private ReadRetryCounter As UInt32 = 0
        Private ReadRetryMax As Long = 1000
        Private ReadAvailableSnapshot As UInt32 = 0
        Private ReadTimeoutStopwatch As Stopwatch = New Stopwatch()
        
        Private ReadOnly WriteLock As System.Object = New System.Object()
        Private WriteRetryResult As UInt32 = 0
        Private WriteRetryCounter As UInt32 = 0
        Private WriteRetryMax As Long = 1000
        Private WriteTimeoutStopwatch As Stopwatch = New Stopwatch()
    End Class
