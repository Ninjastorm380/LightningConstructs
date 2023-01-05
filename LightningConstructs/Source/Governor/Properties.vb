    Public Partial Class Governor
        Public ReadOnly Property Paused As Boolean
            Get
                Return IsPaused
            End Get
        End Property
        Public Property Rate As Double
            Get
                Return BaseRate
            End Get
            Set(ByVal value As Double)
                BaseRate = Rate
                BaseTimeConstant = TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
                BaseSleepTarget = New TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            End Set
        End Property

        Public ReadOnly Property Delta As Double
            Get
                Return Elapsed / Projected
            End Get
        End Property

        Public ReadOnly Property DeltaInverse As Double
            Get
                Return Projected / Elapsed
            End Get
        End Property

        Public ReadOnly Property Elapsed As Double
            Get
                Return BaseDelta / TimeSpan.TicksPerMillisecond
            End Get
        End Property
        Public ReadOnly Property Projected As Double
            Get
                Return BaseTimeConstant / TimeSpan.TicksPerMillisecond
            End Get
        End Property

    End Class