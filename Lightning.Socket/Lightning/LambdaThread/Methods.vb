Imports System.ComponentModel
Friend Partial Class LambdaThread
    Public Sub New(ThreadStart As ThreadStart, Optional Name As String = "New LambdaThread Instance")
        Me.Name = Name
        Workload = ThreadStart
        Worker = New BackgroundWorker()
        AddHandler Worker.DoWork, AddressOf WorkThread
    End Sub

    Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
        Threading.Thread.CurrentThread.Name = Name
        Workload.Invoke()
    End Sub

    Public Sub Start()
        Worker.RunWorkerAsync()
    End Sub
    
    Public Shared Function Start(ThreadStart As ThreadStart, Optional Name As String = "New LambdaThread Instance") As LambdaThread
        Dim NewThread As New LambdaThread(ThreadStart, Name)
        NewThread.Start() : Return NewThread
    End Function
    
    Public Shared Sub Yield()
        Threading.Thread.Yield()
    End Sub
    
    Public Shared Sub Sleep(Duration As TimeSpan)
        Threading.Thread.Sleep(Duration)
    End Sub
End Class

Friend Partial Class LambdaThread(Of T1)
    Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Optional Name As String = "New LambdaThread Instance")
        Me.Name = Name
        P1 = Parameter1
        Workload = ThreadStart
        Worker = New BackgroundWorker()
        AddHandler Worker.DoWork, AddressOf WorkThread
    End Sub

    Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
        Threading.Thread.CurrentThread.Name = Name
        Workload.Invoke(P1)
    End Sub
    
    Public Sub Start()
        Worker.RunWorkerAsync()
    End Sub
    
    Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1)
        Dim NewThread As New LambdaThread(Of T1)(ThreadStart, Parameter1, Name)
        NewThread.Start() : Return NewThread
    End Function
    
End Class