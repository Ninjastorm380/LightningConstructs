    Public Partial Class Governor
        Public Sub New(ByVal Optional Rate As Double = 100.0)
            BaseRate = Rate
            BaseSleepOffsetConstant = TimeSpan.TicksPerMillisecond / 10
            BaseTimeConstant = TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
            BaseSleepTarget = New TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            BaseGovernorWatch = Stopwatch.StartNew()
        End Sub
        
        Public Sub Pause()
            BaseGovernorWatch.Stop()
        End Sub
        
        Public Sub [Resume]()
            BaseGovernorWatch.Start()
        End Sub

        Public Sub Limit()
            TimeTemp = BaseSleepTarget.Ticks - (BaseGovernorWatch.ElapsedTicks + 1000)
            If TimeTemp > 1000 Then
                Threading.Thread.Sleep(BaseSleepTarget)
            End If
            Do While BaseGovernorWatch.ElapsedTicks <= BaseTimeConstant
            Loop 
            BaseDelta = BaseGovernorWatch.ElapsedTicks
            BaseGovernorWatch.Restart()

        End Sub
    End Class