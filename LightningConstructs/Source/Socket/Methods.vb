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
        BaseClientBound = False
    End Sub
    

    
    Private Sub AsyncSocketConnectedMethod(Socket As Socket)
        RaiseEvent SocketConnected(Socket)
    End Sub
    
    Public Sub Connect(ByVal Endpoint As Net.IPEndPoint)
        If IsBaseDisposed = True Then
            NetSocket = New Net.Sockets.Socket(Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        End If
        BaseClientBound = True
        NetSocket.Connect(Endpoint)
        IsConnected = True
        LocalEndPoint = CType(NetSocket.LocalEndPoint, Net.IPEndPoint)
        RemoteEndPoint = CType(NetSocket.RemoteEndPoint, Net.IPEndPoint)
        Dim AsyncThread As Threading.Thread = New Threading.Thread(AddressOf AsyncSocketConnectedMethod)
        AsyncThread.Start(Me)
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
        BaseClientBound = False
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
        
        Dim BytesAvailable As Long = NetSocket.Available
        Dim BytesRead As Long
        Dim TotalBytesRead As Long
        Dim TimeoutMS As Long = 1000
        
        ReadTimeoutStopwatch.Restart()
        If BytesAvailable >= Length Then
            Do
                SyncLock ReadLock
                    BytesRead = NetSocket.Receive(Buffer, TotalBytesRead + Offset, Length - TotalBytesRead, Flags)
                End SyncLock
                TotalBytesRead += BytesRead
            Loop Until ReadTimeoutStopwatch.ElapsedMilliseconds >= TimeoutMS Or TotalBytesRead >= Length Or IsConnected = False
        Else
            Do
                SyncLock ReadLock
                    BytesRead = NetSocket.Receive(Buffer, TotalBytesRead + Offset, BytesAvailable - TotalBytesRead, Flags)
                End SyncLock
                TotalBytesRead += BytesRead
            Loop Until ReadTimeoutStopwatch.ElapsedMilliseconds >= TimeoutMS Or TotalBytesRead >= Length Or IsConnected = False
        End If
        ReadTimeoutStopwatch.Stop()
        
        Return TotalBytesRead
    End Function
    
    Public Function Write(ByRef Buffer As Byte(), ByVal Offset As Int32, ByVal Length As Int32, ByVal Flags As Net.Sockets.SocketFlags) As Int32
        If NetSocket Is Nothing Then Return 0
        If IsConnected = False Then Return 0
        Dim TotalBytesWritten As Long
        SyncLock WriteLock
            TotalBytesWritten = NetSocket.Send(Buffer,  Offset, Length, Flags)
        End SyncLock
        Return TotalBytesWritten
    End Function

    Private Sub AsyncListenerMethod()
        Do
            If NetSocket.Poll(0, Net.Sockets.SelectMode.SelectRead) Then
                Dim NewSocketBase = NetSocket.Accept()
                Dim NewSocket = New Socket()
                NewSocket.HookupSocket(NewSocketBase)
                Dim AsyncThread As System.Threading.Thread = New Threading.Thread(AddressOf AsyncSocketConnectedMethod)
                AsyncThread.Start(NewSocket)
            End If
            ListenerGovernor.Limit()
        Loop While IsListening
    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)>
    Public Sub Dispose() Implements IDisposable.Dispose
        NetSocket.Dispose()
    End Sub
End Class
