Imports System.ComponentModel

Friend Partial Class LambdaThread(Of T1, T2, T3, T4, T5)
    Private ReadOnly Worker As BackgroundWorker
    Private ReadOnly Workload As ThreadStart
    
    Private ReadOnly P1 As T1
    Private ReadOnly P2 As T2
    Private ReadOnly P3 As T3
    Private ReadOnly P4 As T4
    Private ReadOnly P5 As T5
End Class