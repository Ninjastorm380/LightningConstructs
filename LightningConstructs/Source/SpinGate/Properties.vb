    Public Partial Class SpinGate(Of T)
        Public Property Rate As Double
            Get
                Return BaseRate
            End Get
            Set(ByVal value As Double)
                BaseRate = value
            End Set
        End Property
    End Class

