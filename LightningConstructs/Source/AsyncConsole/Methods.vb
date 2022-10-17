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
        Dim Temp As String = ""
        Dim TempArray(32767) As String
        Dim TempArrayCount As Int32
        Do
            If FlushQueue.Length > 0
                
                FlushQueue.ReadOne(Temp,0)
                SyncLock FlushLock
                    FlushQueue.Shift(1)
                End SyncLock
                Console.WriteLine(Temp)
                Threading.Thread.Yield()
            Else
                FlushGovernor.Limit()
            End If
        Loop While ExecutingThread.IsAlive
        If FlushQueue.Length > 0
            TempArrayCount = FlushQueue.Length
            If TempArray.Length < TempArrayCount Then Redim TempArray((TempArrayCount * 2) - 1)
            FlushQueue.Read(TempArray,0,0,TempArrayCount)
            FlushQueue.Shift(TempArrayCount)
            For Index = 0 To TempArrayCount - 1
                Console.WriteLine(TempArray(Index))
            Next
        End If
    End Sub
End Class