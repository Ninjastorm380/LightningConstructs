Imports System.Runtime.CompilerServices

Public Partial Class Socket
    Implements IDisposable

    Public Sub New()
        NetSocket.DualMode = True
        NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
    End Sub

    Public Delegate Sub SocketConnectedHandler(ByVal NewSocket As Socket)

    Public Event SocketConnected As SocketConnectedHandler

    Private Sub HookupSocket(ByVal BaseSocket As Net.Sockets.Socket)
        NetSocket = BaseSocket
        NetSocket.LingerState = New Net.Sockets.LingerOption(True, 30)
        LocalEndPoint = CType(NetSocket.LocalEndPoint, Net.IPEndPoint)
        RemoteEndPoint = CType(NetSocket.RemoteEndPoint, Net.IPEndPoint)
        IsConnected = True
    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)>
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

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Sub Disconnect()
        IsConnected = False
        NetSocket.Shutdown(Net.Sockets.SocketShutdown.Both)
        NetSocket.Disconnect(True)
        NetSocket.Dispose()
        IsBaseDisposed = True
    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Sub Listen(ByVal Endpoint As Net.IPEndPoint)
        NetSocket.Bind(Endpoint)
        NetSocket.Listen(100)
        IsListening = True
        Dim AsyncListener = New Threading.Thread(AddressOf AsyncListenerMethod)
        AsyncListener.Start()
    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Sub Deafen()
        IsListening = False
        NetSocket.Disconnect(True)
    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Function Read(ByRef Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32,
                         ByVal Flags As Net.Sockets.SocketFlags) As Int32
        If NetSocket Is Nothing Then Return 0
        If IsConnected = False Then Return 0
        If NetSocket.Connected = False Then Return 0
        ReadTimeoutStopwatch.Restart()


        ReadRetryCounter = 0
        Do
            ReadRetryResult = 0
            ReadAvailableSnapshot = NetSocket.Available
            If ReadAvailableSnapshot > 0 Then
                SyncLock ReadLock
                    If ReadAvailableSnapshot < Length Then
                        Try
                            ReadRetryResult = NetSocket.Receive(Buffer, ReadRetryCounter + Offset, ReadAvailableSnapshot - ReadRetryCounter, Flags)
                        Catch neterror as Net.Sockets.SocketException
                            ReadRetryResult = 0
                            Disconnect()
                            Exit Do
                        End Try
                    Else
                        Try
                            ReadRetryResult = NetSocket.Receive(Buffer, ReadRetryCounter + Offset, Length - ReadRetryCounter, Flags)
                        Catch neterror as Net.Sockets.SocketException
                            ReadRetryResult = 0
                            Disconnect()
                            Exit Do
                        End Try
                    End If
                End SyncLock
                ReadTimeoutStopwatch.Restart()
            End If
            ReadRetryCounter += ReadRetryResult
        Loop _
            Until _
                ReadRetryCounter = Length Or IsConnected = False Or
                ReadTimeoutStopwatch.ElapsedMilliseconds >= ReadRetryMax
        ReadTimeoutStopwatch.Stop()
        Return ReadRetryCounter
    End Function

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Function Write(ByRef Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32,
                          ByVal Flags As Net.Sockets.SocketFlags) As Int32
        If NetSocket Is Nothing Then Return 0
        If IsConnected = False Then Return 0
        If NetSocket.Connected = False Then Return 0
        WriteTimeoutStopwatch.Restart()


        WriteRetryCounter = 0
        Do
            WriteRetryResult = 0
            SyncLock WriteLock
                Try
                    WriteRetryResult = NetSocket.Send(Buffer, WriteRetryCounter + Offset, Length - WriteRetryCounter,
                                                      Flags)
                    WriteTimeoutStopwatch.Restart()
                Catch neterror as Net.Sockets.SocketException
                    WriteRetryResult = 0
                    Disconnect()
                    Exit Do
                End Try
            End SyncLock
            WriteRetryCounter += WriteRetryResult
        Loop _
            Until _
                WriteRetryCounter = Length Or IsConnected = False Or
                WriteTimeoutStopwatch.ElapsedMilliseconds >= WriteRetryMax
        WriteTimeoutStopwatch.Stop()
        Return WriteRetryCounter
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

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Sub Dispose() Implements IDisposable.Dispose
        NetSocket.Dispose()
    End Sub
End Class
