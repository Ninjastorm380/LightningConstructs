    Public Partial Class Governor
        Public Property Rate As System.Double
            Get
                Return BaseRate
            End Get
            Set(ByVal value As System.Double)
                BaseRate = value
                BaseTimeConstant = System.TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
                BaseSleepTarget = New System.TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            End Set
        End Property

        Public ReadOnly Property Delta As System.Double
            Get
                Return BaseDelta / CDbl(BaseTimeConstant)
            End Get
        End Property

        Public ReadOnly Property DeltaInverse As System.Double
            Get
                Return CDbl(BaseTimeConstant) / BaseDelta
            End Get
        End Property

        Public ReadOnly Property IterationElapsed As System.Double
            Get
                Return (1000.0 / BaseDelta)
            End Get
        End Property

        Public ReadOnly Property IterationProjected As System.Double
            Get
                Return (1000.0 / BaseRate)
            End Get
        End Property
    End Class