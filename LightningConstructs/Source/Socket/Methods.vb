    Public Partial Class Socket : Implements IDisposable
        Public Sub New()
            NetSocket.DualMode = True
            NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
        End Sub

        Public Delegate Sub SocketConnectedHandler(ByVal NewSocket As Socket)
        Public Event SocketConnected As SocketConnectedHandler

        Private Sub HookupSocket(ByVal BaseSocket As Net.Sockets.Socket)
            NetSocket = BaseSocket
            NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
            IsConnected = True
            LocalEndPoint = CType(NetSocket.LocalEndPoint, Net.IPEndPoint)
            RemoteEndPoint = CType(NetSocket.RemoteEndPoint, Net.IPEndPoint)
        End Sub

        Public Sub Connect(ByVal Endpoint As Net.IPEndPoint)
            If IsBaseDisposed = True Then
                NetSocket = New Net.Sockets.Socket(Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
            End If

            NetSocket.Connect(Endpoint)
            IsConnected = True
            LocalEndPoint = CType(NetSocket.LocalEndPoint, Net.IPEndPoint)
            RemoteEndPoint = CType(NetSocket.RemoteEndPoint, Net.IPEndPoint)
            Dim AsyncThread As Threading.Thread = New Threading.Thread(Sub()
                RaiseEvent SocketConnected(Me)
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

        Public Function Read(ByRef Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32, ByVal Flags As Net.Sockets.SocketFlags) As Int32
            If NetSocket Is Nothing Then Return 0
            If IsConnected = False Then Return 0
            If NetSocket.Connected = False Then Return 0

            SyncLock ReadLock
                ReadRetryResult = 0
                ReadRetryCounter = 0
                ReadRetryCurrent = 0
                If ReadGovernor.Paused = True Then ReadGovernor.Resume()
                Do
                    ReadAvailableSnapshot = NetSocket.Available
                    If ReadAvailableSnapshot > 0 Then
                        If ReadAvailableSnapshot < Length Then
                            ReadRetryResult = NetSocket.Receive(Buffer, ReadRetryCounter + Offset, ReadAvailableSnapshot - ReadRetryCounter, Flags)
                        Else
                            ReadRetryResult = NetSocket.Receive(Buffer, ReadRetryCounter + Offset, Length - ReadRetryCounter, Flags)
                        End If
                    Else
                        ReadRetryResult = 0
                    End If

                    ReadRetryCounter += ReadRetryResult
                    If ReadRetryResult > 0 Then
                        ReadRetryCurrent = 0.0
                        If ReadGovernor.Paused = False Then ReadGovernor.Pause()
                    Else 
                        If ReadGovernor.Paused = True Then ReadGovernor.Resume()
                        ReadRetryCurrent += ReadGovernor.Elapsed
                        If ReadRetryCurrent >= ReadRetryMax Then
                            If ReadGovernor.Paused = False Then ReadGovernor.Pause()
                            Return ReadRetryCounter
                        End If
                        ReadGovernor.Limit()
                    End If

                Loop Until ReadRetryCounter = Length Or IsConnected = False
                If ReadGovernor.Paused = False Then ReadGovernor.Pause()
                Return ReadRetryCounter
            End SyncLock
            
            

        End Function

        Public Function Write(ByRef Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32, ByVal Flags As Net.Sockets.SocketFlags) As Int32
            If NetSocket Is Nothing Then Return 0
            If IsConnected = False Then Return 0
            If NetSocket.Connected = False Then Return 0
            SyncLock WriteLock
                WriteRetryResult = 0
                WriteRetryCounter = 0
                WriteRetryCurrent = 0
                If WriteGovernor.Paused = True Then WriteGovernor.Resume()
                Do 
                    Try
                        WriteRetryResult = NetSocket.Send(Buffer, WriteRetryCounter + Offset, Length - WriteRetryCounter, Flags)
                    Catch
                        WriteRetryResult = 0
                    End Try
                    WriteRetryCounter += WriteRetryResult
                    If WriteRetryResult > 0 Then
                        WriteRetryCurrent = 0.0
                        If WriteGovernor.Paused = False Then WriteGovernor.Pause()
                    Else 
                        If WriteGovernor.Paused = True Then WriteGovernor.Resume()
                        WriteRetryCurrent += WriteGovernor.Elapsed
                        If WriteRetryCurrent >= WriteRetryMax Then
                            If WriteGovernor.Paused = False Then WriteGovernor.Pause()
                            Return WriteRetryCounter
                        End If
                        WriteGovernor.Limit()
                    End If
                Loop Until WriteRetryCounter = Length Or IsConnected = False
                If WriteGovernor.Paused = False Then WriteGovernor.Pause()
                Return WriteRetryCounter
            End SyncLock
        End Function

        Private Sub AsyncListenerMethod()
            Do
                If NetSocket.Poll(0, Net.Sockets.SelectMode.SelectRead) Then
                    Dim NewSocketBase = NetSocket.Accept()
                    Dim NewSocket = New Socket()
                    NewSocket.HookupSocket(NewSocketBase)
                    Dim AsyncThread As System.Threading.Thread = New Threading.Thread(Sub()
                        RaiseEvent SocketConnected(NewSocket)
                    End Sub)
                    AsyncThread.Start()
                End If
                ListenerGovernor.Limit()
            Loop While IsListening
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            NetSocket.Dispose()
        End Sub
    End Class
