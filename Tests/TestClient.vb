Imports Lightning
Public Class TestClient
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
    Public Sub New()
        AddHandler Socket.ClientSocketConnected, AddressOf ConnectedEvent
    End Sub
    Public Sub Connect
        Socket.Connect(Endpoint)
    End Sub
    Public Sub Disconnect
        Socket.Disconnect()
    End Sub
    Public Sub TestReusability()
        Console.WriteLine("Testing client socket reusability...")
        Socket.Connect(Endpoint)
        If Socket.Connected = True Then
            Console.WriteLine("Client socket connected.")
        Else
            Console.WriteLine("Client socket failed to connect.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Disconnect()
        If Socket.Connected = False Then
            Console.WriteLine("Client socket disconnected.")
        Else
            Console.WriteLine("Client socket failed to disconnect.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Connect(Endpoint)
        If Socket.Connected = True Then
            Console.WriteLine("Client socket connected again.")
        Else
            Console.WriteLine("Client socket failed to connect again.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Disconnect()
        If Socket.Connected = False Then
            Console.WriteLine("Client socket disconnected again.")
        Else
            Console.WriteLine("Client socket failed to disconnect again.")
        End If
        
        Console.WriteLine("Test complete.")
        Threading.Thread.Sleep(1000)
    End Sub
    
    Private Sub ConnectedEvent(NewSocket As Socket)
        Console.WriteLine("  Client: Connected.")
        Dim Governor as new Governor(1000)
        Dim BufferIn(3) As Byte
        Dim BufferInTest As Byte() = {1,2,3,4}
        NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
        While NewSocket.Connected = True
            If NewSocket.Available > 0 Then
                BufferIn(0) = 0
                BufferIn(1) = 0
                BufferIn(2) = 0
                BufferIn(3) = 0
                If NewSocket.Read(BufferIn,0, 4, Net.Sockets.SocketFlags.None) > 0
                    If BufferIn(0) <> BufferInTest(0) Then
                        Console.WriteLine("  Client: Data error at byte 0.")
                    End If
                    If BufferIn(1) <> BufferInTest(1) Then
                        Console.WriteLine("  Client: Data error at byte 1.")
                    End If
                    If BufferIn(2) <> BufferInTest(2) Then
                        Console.WriteLine("  Client: Data error at byte 2.")
                    End If
                    If BufferIn(3) <> BufferInTest(3) Then
                        Console.WriteLine("  Client: Data error at byte 3.")
                    End If
                    NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
                End If
            End If
            Governor.Limit()
        End While
        Console.WriteLine("  Client: Disconnected.")
    End Sub
End Class