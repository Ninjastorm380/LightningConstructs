Imports System.ComponentModel

Friend Partial Class LambdaThread
    Private ReadOnly Worker As BackgroundWorker
    Private ReadOnly Workload As ThreadStart
End Class

Friend Partial Class LambdaThread(Of T1)
    Private ReadOnly Worker As BackgroundWorker
    Private ReadOnly Workload As ThreadStart
    
    Private ReadOnly P1 As T1
End Class