Public Partial Class QueueStream(Of T)
    Public ReadOnly Property Length As Int32
        Get
            Return WritePointer
        End Get
    End Property
    Public ReadOnly Property Buffer As T()
        Get
            Return BaseBuffer
        End Get
    End Property


End Class