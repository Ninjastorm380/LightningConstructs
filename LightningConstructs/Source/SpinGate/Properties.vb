    Public Partial Class SpinGate(Of T)
        Public Property Rate As System.Double
            Get
                Return BaseRate
            End Get
            Set(ByVal value As System.Double)
                BaseRate = value
            End Set
        End Property
    End Class

