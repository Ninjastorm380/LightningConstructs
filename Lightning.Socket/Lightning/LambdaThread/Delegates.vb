Friend Partial Class LambdaThread
    Public Delegate Sub ThreadStart
End Class

Friend Partial Class LambdaThread(Of T1)
    Public Delegate Sub ThreadStart(Parameter1 As T1)
End Class