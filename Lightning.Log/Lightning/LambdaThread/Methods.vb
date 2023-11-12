Imports System.ComponentModel
Friend Partial Class LambdaThread(Of T1, T2, T3, T4, T5)
    Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Optional Name As String = "New LambdaThread Instance")
        Me.Name = Name
        P1 = Parameter1
        P2 = Parameter2
        P3 = Parameter3
        P4 = Parameter4
        P5 = Parameter5
        Workload = ThreadStart
        Worker = New BackgroundWorker()
        AddHandler Worker.DoWork, AddressOf WorkThread
    End Sub

    Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
        Threading.Thread.CurrentThread.Name = Name
        Workload.Invoke(P1, P2, P3, P4, P5)
    End Sub
    
    Public Sub Start()
        Worker.RunWorkerAsync()
    End Sub
    
    Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2, T3, T4, T5)
        Dim NewThread As New LambdaThread(Of T1, T2, T3, T4, T5)(ThreadStart, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Name)
        NewThread.Start() : Return NewThread
    End Function
End Class