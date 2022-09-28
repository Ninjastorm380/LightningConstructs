Imports Lightning
Public Class TestClient
    Private ReadOnly Socket As New Socket
    Public Property Endpoint As Net.IPEndPoint
    Public Sub New()
        AddHandler Socket.ClientSocketConnected, AddressOf ListenerEvent
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
    
    Private Sub ListenerEvent(NewSocket As Socket)
        Console.WriteLine("Client: Connected.")
    End Sub
End Class