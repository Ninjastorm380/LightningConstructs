Imports Lightning
Public Class TestServer : Implements IDisposable
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
    Private Readonly Console As New AsyncConsole
    Public Sub New()
        AddHandler Socket.ServerSocketConnected, AddressOf ConnectedEvent
    End Sub
    Public Sub Start
        Socket.Listen(Endpoint)
    End Sub
    Public Sub [Stop]
        Socket.Deafen()
    End Sub
    
    Public Sub Test()
        
        Console.WriteLine("Testing server socket listener reusability...")
        Console.WriteLine("Starting server socket listener...")
        Socket.Listen(Endpoint)
        If Socket.Listening = True Then
            Console.WriteLine("Server socket listener started.")
        Else
            Console.WriteLine("Server socket listener failed to start.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Stopping server socket listener...")
        Socket.Deafen()
        If Socket.Listening = False Then
            Console.WriteLine("Server socket listener stopped.")
        Else
            Console.WriteLine("Server socket listener failed to stop.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Starting server socket listener...")
        Socket.Listen(Endpoint)
        If Socket.Listening = True Then
            Console.WriteLine("Server socket listener started.")
        Else
            Console.WriteLine("Server socket listener failed to start.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Console.WriteLine("Stopping server socket listener...")
        Socket.Deafen()
        If Socket.Listening = False Then
            Console.WriteLine("Server socket listener stopped.")
        Else
            Console.WriteLine("Server socket listener failed to stop.")
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
                Console.WriteLine("  Server: Reading in data...")
                If NewSocket.Read(BufferIn,0, 4, Net.Sockets.SocketFlags.None) > 0
                    Console.WriteLine("  Server: Read in data. checking...")
                    Trip = False
                    If BufferIn(0) <> BufferInTest(0) Then
                        Console.WriteLine("  Server: Data error at byte 0.")
                        Trip = True
                    End If
                    If BufferIn(1) <> BufferInTest(1) Then
                        Console.WriteLine("  Server: Data error at byte 1.")
                        Trip = True
                    End If
                    If BufferIn(2) <> BufferInTest(2) Then
                        Console.WriteLine("  Server: Data error at byte 2.")
                        Trip = True
                    End If
                    If BufferIn(3) <> BufferInTest(3) Then
                        Console.WriteLine("  Server: Data error at byte 3.")
                        Trip = True
                    End If
                    If Trip = False Then
                        Console.WriteLine("  Server: data integrity verified. Passed.")
                    Else
                        Console.WriteLine("  Server: data integrity failed. Check code.")
                    End If
                    Console.WriteLine("  Server: writing out data...")
                    If NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None) > 0
                        Console.WriteLine("  Server: Data written.")
                    Else
                        If NewSocket.Connected = False Then
                            Console.WriteLine("  Server: Data failed to write. Socket is not connected. Expected failure encountered in test.")
                        Else
                            Console.WriteLine("  Server: Data failed to write. Socket is still connected. Check code.")
                        End If
                    End If
                End If
            End If
            Governor.Limit()
        End While
        Console.WriteLine("  Server: Disconnected.")
    End Sub
    
    Public Sub Dispose() Implements IDisposable.Dispose
        Console.Dispose()
    End Sub
End Class