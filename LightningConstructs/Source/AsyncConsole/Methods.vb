Public Partial Class AsyncConsole : Implements IDisposable
    Public Sub New(Optional Rate As Double = 20.0)
        FlushGovernor.Rate = Rate
        CloseFlushThread = False
        SyncLock FlushLock
            If BufferEngineExists = False Then
                Dim AsyncThread As New Threading.Thread( AddressOf FlushEngine)
                AsyncThread.Start()
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
                    If CloseFlushThread = True Then Exit For
                    Console.WriteLine(TempArray(Index))
                    Threading.Thread.Yield()
                Next
            End If
            FlushGovernor.Limit()
        Loop While CloseFlushThread = False
        SyncLock FlushLock
            FlushQueue.Clear()
        End SyncLock
    End Sub
    
    Public Sub Dispose() Implements IDisposable.Dispose
        CloseFlushThread = True
        SyncLock FlushLock
            FlushQueue.Clear()
        End SyncLock
    End Sub
End Class