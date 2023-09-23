Imports System.Net.Sockets
Imports Lightning

Public Class TestClient
    Private ReadOnly Socket As Socket
    Public Property Endpoint As Net.IPEndPoint

    Public Sub New()
        Socket = New Socket()
        AddHandler Socket.OnConnect, AddressOf ConnectedEvent
        AddHandler Socket.OnDisconnect, AddressOf DisconnectedEvent
    End Sub
    
    Public Sub Connect()
        Socket.Connect(Endpoint, SocketType.Stream, ProtocolType.Tcp)
    End Sub

    Public Sub Disconnect()
        Socket.Disconnect()
    End Sub

    Private Sub ConnectedEvent(NewSocket As Socket)
        Console.WriteLine("  client: connected.")
        LambdaThread (Of Socket).Start(AddressOf Main, NewSocket, "Client Socket Test Method")
    End Sub

    Private Sub DisconnectedEvent(NewSocket As Socket)
        Console.WriteLine("  client: disconnected.")
    End Sub

    Private Sub Main(NewSocket As Socket)
        Dim Governor as new Governor(1000)
        Dim BufferIn(3) As Byte
        Dim BufferInTest As Byte() = {1, 2, 3, 4}
        Dim Trip As Boolean = False
        Console.WriteLine("  Client: Writing test data.")
        NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None, Nothing)
        Console.WriteLine("  Client: Test data written.")
        While NewSocket.Connected = True
            If NewSocket.Available > 0 Then
                BufferIn(0) = 0
                BufferIn(1) = 0
                BufferIn(2) = 0
                BufferIn(3) = 0
                Console.WriteLine("  Client: Reading in data...")
                If NewSocket.Read(BufferIn, 0, 4, Net.Sockets.SocketFlags.None, Nothing) > 0
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
                    If NewSocket.Connected = True
                        If NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None, Nothing) = 4
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
            End If
            Governor.Limit()
        End While
    End Sub
End Class