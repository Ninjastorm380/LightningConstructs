    Public Partial Class Socket
        Public ReadOnly Property Available As Int32
            Get
                Return NetSocket.Available
            End Get
        End Property

        Public ReadOnly Property Listening As System.Boolean
            Get
                Return IsListening
            End Get
        End Property

        Public ReadOnly Property Connected As System.Boolean
            Get
                If IsConnected = False Then Return False
                SyncLock ReadLock
                    Dim DataAvailable = NetSocket.Poll(0, Net.Sockets.SelectMode.SelectRead)
                    Dim DataNotAvailable = (NetSocket.Available = 0)
                    IsConnected = Not (DataAvailable = True AndAlso DataNotAvailable = True)
                    Return IsConnected
                End SyncLock
            End Get
        End Property
    End Class
