Imports Lightning
Public Class TestClient
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
    Private Readonly Console As New AsyncConsole
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
        Console.WriteLine("Connecting client socket...")
        Socket.Connect(Endpoint)
        If Socket.Connected = True Then
            Console.WriteLine("Client socket connected.")
        Else
            Console.WriteLine("Client socket failed to connect.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Disconnecting client socket...")
        Socket.Disconnect()
        If Socket.Connected = False Then
            Console.WriteLine("Client socket disconnected.")
        Else
            Console.WriteLine("Client socket failed to disconnect.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Connecting client socket...")
        Socket.Connect(Endpoint)
        If Socket.Connected = True Then
            Console.WriteLine("Client socket connected.")
        Else
            Console.WriteLine("Client socket failed to connect.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Disconnecting client socket...")
        Socket.Disconnect()
        If Socket.Connected = False Then
            Console.WriteLine("Client socket disconnected.")
        Else
            Console.WriteLine("Client socket failed to disconnect.")
        End If
        
        Console.WriteLine("Subtest complete.")
        Threading.Thread.Sleep(1000)
    End Sub
    
    Private Sub ConnectedEvent(NewSocket As Socket)
        Console.WriteLine("  Client: Connected.")
        Dim Governor as new Governor(1000)
        Dim BufferIn(3) As Byte
        Dim BufferInTest As Byte() = {1,2,3,4}
        Dim Trip As Boolean = False
        NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
        While NewSocket.Connected = True
            If NewSocket.Available > 0 Then
                BufferIn(0) = 0
                BufferIn(1) = 0
                BufferIn(2) = 0
                BufferIn(3) = 0
                Console.WriteLine("  Client: Reading in data...")
                If NewSocket.Read(BufferIn,0, 4, Net.Sockets.SocketFlags.None) > 0
                    Console.WriteLine("  Client: Read in data. checking...")
                    Trip = False
                    If BufferIn(0) <> BufferInTest(0) Then
                        Console.WriteLine("  Client: Data error at byte 0.")
                        Trip = True
                    End If
                    If BufferIn(1) <> BufferInTest(1) Then
                        Console.WriteLine("  Client: Data error at byte 1.")
                        Trip = True
                    End If
                    If BufferIn(2) <> BufferInTest(2) Then
                        Console.WriteLine("  Client: Data error at byte 2.")
                        Trip = True
                    End If
                    If BufferIn(3) <> BufferInTest(3) Then
                        Console.WriteLine("  Client: Data error at byte 3.")
                        Trip = True
                    End If
                    If Trip = False Then
                        Console.WriteLine("  Client: data integrity verified. Passed.")
                    Else
                        Console.WriteLine("  Client: data integrity failed. Check code.")
                    End If
                    Console.WriteLine("  Client: writing out data...")
                    If NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None) > 0
                        Console.WriteLine("  Client: Data written.")
                    Else
                        If NewSocket.Connected = False Then
                            Console.WriteLine("  Client: Data failed to write. Socket is not connected. Expected failure encountered in test.")
                        Else
                            Console.WriteLine("  Client: Data failed to write. Socket is still connected. Check code.")
                        End If
                    End If
                    
                End If
            End If
            Governor.Limit()
        End While
        Console.WriteLine("  Client: Disconnected.")
    End Sub
End Class