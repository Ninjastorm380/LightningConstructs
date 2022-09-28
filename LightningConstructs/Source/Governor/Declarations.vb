
    Public Partial Class Governor
        Private BaseRate As System.Double
        Private BaseTimeConstant As System.Int64
        Private BaseDelta As System.Int64
        Private ReadOnly BaseGovernorWatch As System.Diagnostics.Stopwatch
        Private ReadOnly BaseSleepOffsetConstant As System.Int64
        Private BaseSleepTarget As System.TimeSpan
    End Class

