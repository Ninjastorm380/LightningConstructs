Imports Lightning
Public Class TestServer
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
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
        Socket.Listen(Endpoint)
        If Socket.Listening = True Then
            Console.WriteLine("Server socket listener started.")
        Else
            Console.WriteLine("Server socket listener failed to start.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Deafen()
        If Socket.Listening = False Then
            Console.WriteLine("Server socket listener stopped.")
        Else
            Console.WriteLine("Server socket listener failed to stop.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Listen(Endpoint)
        If Socket.Listening = True Then
            Console.WriteLine("Server socket listener started again.")
        Else
            Console.WriteLine("Server socket listener failed to start again.")
        End If
        
        Threading.Thread.Sleep(1000)
        
        Socket.Deafen()
        If Socket.Listening = False Then
            Console.WriteLine("Server socket listener stopped again.")
        Else
            Console.WriteLine("Server socket listener failed to stop again.")
        End If
        
        Console.WriteLine("Test complete.")
        Threading.Thread.Sleep(1000)
    End Sub
    
    Private Sub ConnectedEvent(NewSocket As Socket)
        Console.WriteLine("  Server: Client connected.")
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
                        Console.WriteLine("  Server: Data error at byte 0.")
                    End If
                    If BufferIn(1) <> BufferInTest(1) Then
                        Console.WriteLine("  Server: Data error at byte 1.")
                    End If
                    If BufferIn(2) <> BufferInTest(2) Then
                        Console.WriteLine("  Server: Data error at byte 2.")
                    End If
                    If BufferIn(3) <> BufferInTest(3) Then
                        Console.WriteLine("  Server: Data error at byte 3.")
                    End If
                    NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
                End If
            End If
            Governor.Limit()
        End While
        Console.WriteLine("  Server: Client disconnected.")
    End Sub
End Class