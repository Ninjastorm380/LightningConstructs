    Public Partial Class QueueStream(Of T)
        Private BaseBuffer As T() = New T(65535) {}
        Private WritePointer As System.Int32 = 0
    End Class
