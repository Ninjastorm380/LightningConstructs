Public Partial Class AsyncConsole
    Public Sub New(Optional Rate As Double = 20.0)
        FlushGovernor.Rate = Rate
        SyncLock FlushLock
            If BufferEngineExists = False Then
                Dim AsyncThread As New Threading.Thread( AddressOf FlushEngine)
                AsyncThread.Start()
                ExecutingThread = Threading.Thread.CurrentThread
                BufferEngineExists = True
            End If
        End SyncLock

    End Sub
    
    Public Sub WriteLine(Line As String)
        SyncLock FlushLock
            FlushQueue.WriteOne(Line)
        End SyncLock
    End Sub

    Private Sub FlushEngine()
        Dim TempArray(0) As String
        Dim TempArrayCount As Int32
        Do
            If FlushQueue.Length > 0
                TempArrayCount = FlushQueue.Length
                If TempArray.Length < TempArrayCount Then Redim TempArray(TempArrayCount - 1)
                
                SyncLock FlushLock
                    FlushQueue.Read(TempArray,0,0,TempArrayCount)
                    FlushQueue.Shift(TempArrayCount)
                End SyncLock
                For Index = 0 To TempArrayCount - 1
                    If ExecutingThread.IsAlive = False Then Exit For
                    Console.WriteLine(TempArray(Index))
                    Threading.Thread.Yield()
                Next
            End If
            FlushGovernor.Limit()
        Loop While ExecutingThread.IsAlive
        SyncLock FlushLock
            FlushQueue.Clear()
        End SyncLock
    End Sub
End Class