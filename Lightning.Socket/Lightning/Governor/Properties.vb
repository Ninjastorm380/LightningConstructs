Friend Partial Class Governor
    Public Property Frequency As Double
        Get
            Return BaseFrequency
        End Get
        Set
            BaseFrequency = Value
            TargetTimespan = FrequencyToTimespan(Value)
            SleepTimespan = FrequencyToTimespan(Value - 1.0)
            If SleepTimespan.TotalMilliseconds >= 1 Then DoSleep = True Else DoSleep = False
        End Set
    End Property
    
    Public Property Delta As TimeSpan
End Class