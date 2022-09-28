    Public Partial Class Socket
        Public Sub New()
            NetSocket.DualMode = True
            NetSocket.LingerState = New System.Net.Sockets.LingerOption(True, 30)
        End Sub

        Public Delegate Sub SocketConnectedHandler(ByVal NewSocket As Socket)
        Public Event ServerSocketConnected As SocketConnectedHandler
        Public Event ClientSocketConnected As SocketConnectedHandler

        Private Sub HookupSocket(ByVal BaseSocket As System.Net.Sockets.Socket)
            NetSocket = BaseSocket
            NetSocket.LingerState = New System.Net.Sockets.LingerOption(True, 30)
            IsConnected = True
        End Sub

        Public Sub Connect(ByVal Endpoint As System.Net.IPEndPoint)
            If IsBaseDisposed = True Then
                NetSocket = New System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
            End If

            NetSocket.Connect(Endpoint)
            IsConnected = True

            Dim AsyncThread As System.Threading.Thread = New System.Threading.Thread(Sub(AsyncThreadMethod)
                RaiseEvent ClientSocketConnected(Me)
            End Sub)
            AsyncThread.Start()
        End Sub

        Public Sub Disconnect()
            IsConnected = False
            NetSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both)
            NetSocket.Disconnect(True)
            NetSocket.Dispose()
            IsBaseDisposed = True
        End Sub

        Public Sub Listen(ByVal Endpoint As System.Net.IPEndPoint)
            NetSocket.Bind(Endpoint)
            NetSocket.Listen(100)
            IsListening = True
            Dim AsyncListener = New System.Threading.Thread(AddressOf AsyncListenerMethod)
            AsyncListener.Start()
        End Sub

        Public Sub Deafen()
            IsListening = False
            NetSocket.Disconnect(True)
        End Sub

        Public Function Read(ByVal Buffer As System.Byte(), ByVal Offset As System.Int32, ByVal Length As System.Int32, ByVal Flags As System.Net.Sockets.SocketFlags) As System.Int32
            If Connected = False Then
                Return 0
            End If

            SyncLock ReadLock
                Return NetSocket.Receive(Buffer, Offset, Length, Flags)
            End SyncLock
        End Function

        Public Function Write(ByVal Buffer As System.Byte(), ByVal Offset As System.Int32, ByVal Length As System.Int32, ByVal Flags As System.Net.Sockets.SocketFlags) As System.Int32
            If Connected = False Then
                Return 0
            End If

            SyncLock WriteLock
                Return NetSocket.Send(Buffer, Offset, Length, Flags)
            End SyncLock
        End Function

        Private Sub AsyncListenerMethod()
            Do

                If NetSocket.Poll(0, System.Net.Sockets.SelectMode.SelectRead) Then
                    Dim NewSocketBase = NetSocket.Accept()
                    Dim NewSocket = New Socket()
                    NewSocket.HookupSocket(NewSocketBase)

                    Dim AsyncThread As System.Threading.Thread = New System.Threading.Thread(Sub(AsyncThreadMethod)
                        RaiseEvent ServerSocketConnected(NewSocket)
                    End Sub)
                    AsyncThread.Start()
                End If

                ListenerGovernor.Limit()
            Loop While IsListening
        End Sub
    End Class
