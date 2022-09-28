Imports Lightning
Public Class TestServer
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
    Public Sub New()
        AddHandler Socket.ServerSocketConnected, AddressOf ListenerEvent
    End Sub
    Public Sub Start
        Socket.Listen(Endpoint)
    End Sub
    Public Sub [Stop]
        Socket.Deafen()
    End Sub
    
    Public Sub TestReusability()
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
    
    Private Sub ListenerEvent(NewSocket As Socket)
        Console.WriteLine("  Server: Client connected.")
        Dim Governor as new Governor(20)
        Dim BufferIn(3) As Byte
        Dim BufferInTest As Byte() = {1,2,3,4}
        NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
        While NewSocket.Connected = True
            If NewSocket.Available > 0 Then
                BufferIn(0) = 0
                BufferIn(1) = 0
                BufferIn(2) = 0
                BufferIn(3) = 0
                Console.WriteLine("  Server: Reading in data...")
                If NewSocket.Connected = True Then NewSocket.Read(BufferIn,0, 4, Net.Sockets.SocketFlags.None)
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
                Console.WriteLine("  Server: Writing out data...")
                If NewSocket.Connected = True Then NewSocket.Write(BufferInTest, 0, 4, Net.Sockets.SocketFlags.None)
            End If
            Governor.Limit()
        End While
        Console.WriteLine("  Server: Client disconnected.")
    End Sub
End Class