    Public Partial Class Socket
        Public Sub New()
            NetSocket.DualMode = True
            NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
        End Sub

        Public Delegate Sub SocketConnectedHandler(ByVal NewSocket As Socket)
        Public Event ServerSocketConnected As SocketConnectedHandler
        Public Event ClientSocketConnected As SocketConnectedHandler

        Private Sub HookupSocket(ByVal BaseSocket As Net.Sockets.Socket)
            NetSocket = BaseSocket
            NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
            IsConnected = True
        End Sub

        Public Sub Connect(ByVal Endpoint As Net.IPEndPoint)
            If IsBaseDisposed = True Then
                NetSocket = New Net.Sockets.Socket(Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
            End If

            NetSocket.Connect(Endpoint)
            IsConnected = True

            Dim AsyncThread As Threading.Thread = New Threading.Thread(Sub(AsyncThreadMethod)
                RaiseEvent ClientSocketConnected(Me)
            End Sub)
            AsyncThread.Start()
        End Sub

        Public Sub Disconnect()
            IsConnected = False
            NetSocket.Shutdown(Net.Sockets.SocketShutdown.Both)
            NetSocket.Disconnect(True)
            NetSocket.Dispose()
            IsBaseDisposed = True
        End Sub

        Public Sub Listen(ByVal Endpoint As Net.IPEndPoint)
            NetSocket.Bind(Endpoint)
            NetSocket.Listen(100)
            IsListening = True
            Dim AsyncListener = New Threading.Thread(AddressOf AsyncListenerMethod)
            AsyncListener.Start()
        End Sub

        Public Sub Deafen()
            IsListening = False
            NetSocket.Disconnect(True)
        End Sub

        Public Function Read(ByVal Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32, ByVal Flags As Net.Sockets.SocketFlags) As Int32
            If Connected = False Then
                Return 0
            End If

            SyncLock ReadLock
                Return NetSocket.Receive(Buffer, Offset, Length, Flags)
            End SyncLock
        End Function

        Public Function Write(ByVal Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32, ByVal Flags As Net.Sockets.SocketFlags) As Int32
            SyncLock WriteLock
               If Connected = True Then Return NetSocket.Send(Buffer, Offset, Length, Flags)
            End SyncLock
            Return 0
        End Function

        Private Sub AsyncListenerMethod()
            Do
                If NetSocket.Poll(0, Net.Sockets.SelectMode.SelectRead) Then
                    Dim NewSocketBase = NetSocket.Accept()
                    Dim NewSocket = New Socket()
                    NewSocket.HookupSocket(NewSocketBase)
                    Dim AsyncThread As System.Threading.Thread = New Threading.Thread(Sub(AsyncThreadMethod)
                        RaiseEvent ServerSocketConnected(NewSocket)
                    End Sub)
                    AsyncThread.Start()
                End If
                ListenerGovernor.Limit()
            Loop While IsListening
        End Sub
    End Class
