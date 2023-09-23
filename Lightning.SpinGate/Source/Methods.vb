Public Partial Class SpinGate
    Public Sub New()
        LockObject = New Object
    End Sub
    
    Public Function Lock() As Boolean
        Dim SpinWatchdog As Stopwatch = Stopwatch.StartNew()
        SyncLock LockObject
            LockFlag = True
        End SyncLock
        
        If LockFlag = True
            Dim SpinGate As New Governor(100)
            Do Until LockFlag = False Or SpinWatchdog.Elapsed >= Timeout
                SpinGate.Limit()
            Loop
        End If
        
        Return LockFlag
    End Function
    
    Public Sub Unlock()
        SyncLock LockObject
            LockFlag = False
        End SyncLock
    End Sub
End Class

Public Partial Class SpinGate(Of T)
    Public Sub New()
        LockObject = New Object
        Result = Nothing
    End Sub
    
    Public Function Lock() As T
        Dim SpinWatchdog As Stopwatch = Stopwatch.StartNew()
        SyncLock LockObject
            Result = Nothing
            LockFlag = True
        End SyncLock
        
        If LockFlag = True
            Dim SpinGate As New Governor(200)
            Do Until LockFlag = False Or SpinWatchdog.Elapsed >= Timeout
                SpinGate.Limit()
            Loop
        End If
        
        If LockFlag = True Then Return Nothing
        Return Result
    End Function
    
    Public Sub Unlock(Value As T)
        SyncLock LockObject
            Result = Value
            LockFlag = False
        End SyncLock
    End Sub
End Class