Imports System.ComponentModel
Namespace Lightning
    Public Partial Class LambdaThread
    
        Public Sub New(ThreadStart As ThreadStart, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke()
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub

        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
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

    Public Partial Class LambdaThread(Of T1)
        Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            P1 = Parameter1
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
    
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1)
            Dim NewThread As New LambdaThread(Of T1)(ThreadStart, Parameter1, Name)
            NewThread.Start() : Return NewThread
        End Function
    
    End Class

    Public Partial Class LambdaThread(Of T1, T2)
        Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            P1 = Parameter1
            P2 = Parameter2
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1, P2)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
    
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1, Parameter2 As T2)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                P2 = Parameter2
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2)
            Dim NewThread As New LambdaThread(Of T1, T2)(ThreadStart, Parameter1, Parameter2, Name)
            NewThread.Start() : Return NewThread
        End Function
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3)
        Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            P1 = Parameter1
            P2 = Parameter2
            P3 = Parameter3
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1, P2, P3)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
    
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1, Parameter2 As T2, Parameter3 As T3)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                P2 = Parameter2
                P3 = Parameter3
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2, T3)
            Dim NewThread As New LambdaThread(Of T1, T2, T3)(ThreadStart, Parameter1, Parameter2, Parameter3, Name)
            NewThread.Start() : Return NewThread
        End Function
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4)
        Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            P1 = Parameter1
            P2 = Parameter2
            P3 = Parameter3
            P4 = Parameter4
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1, P2, P3, P4)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
        
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                P2 = Parameter2
                P3 = Parameter3
                P4 = Parameter4
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2, T3, T4)
            Dim NewThread As New LambdaThread(Of T1, T2, T3, T4)(ThreadStart, Parameter1, Parameter2, Parameter3, Parameter4, Name)
            NewThread.Start() : Return NewThread
        End Function
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5)
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
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1, P2, P3, P4, P5)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
    
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                P2 = Parameter2
                P3 = Parameter3
                P4 = Parameter4
                P5 = Parameter5
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2, T3, T4, T5)
            Dim NewThread As New LambdaThread(Of T1, T2, T3, T4, T5)(ThreadStart, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Name)
            NewThread.Start() : Return NewThread
        End Function
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5, T6)
        Public Sub New(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Parameter6 As T6, Optional Name As String = "New LambdaThread Instance")
            Me.Name = Name
            P1 = Parameter1
            P2 = Parameter2
            P3 = Parameter3
            P4 = Parameter4
            P5 = Parameter5
            P6 = Parameter6
            Workload = ThreadStart
            Worker = New BackgroundWorker()
            AddHandler Worker.DoWork, AddressOf WorkThread
        End Sub

        Private Sub WorkThread(Sender As Object, E As DoWorkEventArgs)
            Dim OldName As String = Threading.Thread.CurrentThread.Name
            Threading.Thread.CurrentThread.Name = Name
            Workload.Invoke(P1, P2, P3, P4, P5, P6)
            Threading.Thread.CurrentThread.Name = OldName
            BaseRunning = False
        End Sub
    
        Public Sub Start()
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
        
        Public Sub Start(Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Parameter6 As T6)
            If BaseRunning = True Then Return
            SyncLock Worker
                If BaseRunning = True Then Return
                BaseRunning = True
                P1 = Parameter1
                P2 = Parameter2
                P3 = Parameter3
                P4 = Parameter4
                P5 = Parameter5
                P6 = Parameter6
                Worker.RunWorkerAsync()
            End SyncLock
        End Sub
    
        Public Shared Function Start(ThreadStart As ThreadStart, Parameter1 As T1, Parameter2 As T2, Parameter3 As T3, Parameter4 As T4, Parameter5 As T5, Parameter6 As T6, Optional Name As String = "New LambdaThread Instance") As LambdaThread(Of T1, T2, T3, T4, T5, T6)
            Dim NewThread As New LambdaThread(Of T1, T2, T3, T4, T5, T6)(ThreadStart, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Name)
            NewThread.Start() : Return NewThread
        End Function
    End Class
End Namespace
