    Public Partial Class Governor
        Public Sub New(ByVal Optional Rate As System.Double = 100.0)
            BaseRate = Rate
            BaseSleepOffsetConstant = System.TimeSpan.TicksPerMillisecond / 10
            BaseTimeConstant = System.TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
            BaseSleepTarget = New System.TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            BaseGovernorWatch = System.Diagnostics.Stopwatch.StartNew()
        End Sub

        Public Sub Limit()
            If BaseSleepTarget.Ticks > 1000 Then
                System.Threading.Thread.Sleep(BaseSleepTarget)
            End If

            Do
            Loop While BaseGovernorWatch.ElapsedTicks < BaseTimeConstant

            BaseDelta = BaseGovernorWatch.ElapsedTicks
            BaseGovernorWatch.Restart()
        End Sub
    End Class