Public Partial Class Governor
    Public Sub New(Optional Frequency As UInt16 = 60)
        BaseFrequency = Frequency
        DeltaTicks = 0
        BaseDelta = 0
        GovernorWatch = Stopwatch.StartNew()
    End Sub
    
    Public Sub Limit()
        BaseDelta = DeltaTicks / 10000
        Do Until GovernorWatch.ElapsedTicks >= 10000000/BaseFrequency
        Loop 
        DeltaTicks = GovernorWatch.ElapsedTicks
        GovernorWatch.Restart()
    End Sub
End Class