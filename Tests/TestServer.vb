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
        Console.WriteLine("Server: Client socket has connected.")
    End Sub
End Class