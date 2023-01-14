Public Class Governor
    Private GovernorWatch As Stopwatch
    Private DeltaTicks As Long
    Private BaseDelta As Double
    Private BaseFrequency As UInt16
    Private WaitTimespan As TimeSpan
    Public Property Frequency As UInt16
        Get
            Return BaseFrequency
        End Get
        Set
            BaseFrequency = Value
            WaitTimespan = New TimeSpan(10000000/BaseFrequency)
        End Set
    End Property
    
    Public ReadOnly Property Delta As Double
        Get
            Return BaseDelta
        End Get
    End Property
    
    Public Sub New(Optional Frequency As UInt16 = 60)
        BaseFrequency = Frequency
        WaitTimespan = New TimeSpan(10000000/BaseFrequency)
        DeltaTicks = 0
        BaseDelta = 0
        GovernorWatch = Stopwatch.StartNew()
    End Sub
    
    Public Sub Limit()
        BaseDelta = DeltaTicks / TimeSpan.TicksPerMillisecond
        Do Until GovernorWatch.ElapsedTicks >= WaitTimespan.Ticks
        Loop 
        DeltaTicks = GovernorWatch.ElapsedTicks
        GovernorWatch.Restart()
    End Sub
End Class