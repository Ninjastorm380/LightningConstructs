Namespace Lightning
    Public Partial Class ParallelWorkerEngine(Of Worker As {ParallelWorker})
        Public Sub New()
            Init(524288)
        End Sub
        
        Public Sub New(Capacity As Int32)
            Init(Capacity)
        End Sub
        
        Private Sub Init(Capacity As Int32)
            ConcurrentMarcher = New Queue(Of Worker)(Capacity)
            ConcurrentIterator = New List(Of Worker)(Capacity)
        End Sub
        
        Public Sub Enqueue(Worker As Worker)
            ConcurrentMarcher.Enqueue(Worker)
            ConcurrentIterator.Add(Worker)
        End Sub
        
        Public Function Dequeue() As Worker
            Dim DequeuedWorker As Worker = ConcurrentMarcher.Dequeue()
            ConcurrentIterator.Remove(DequeuedWorker)
            Return DequeuedWorker
        End Function
        
        Public Sub Run()
            For Each Item In ConcurrentMarcher
                Item.Run()
            Next
        End Sub
        
        Public Sub WaitForAll()
            Dim Completed As Boolean
            Do 
                Completed = True
                For Each Item In ConcurrentIterator
                    If Item.Completed = False Then
                        Completed = False : Exit For
                    End If
                Next
            Loop Until Completed = True
        End Sub
        
        Public Sub Clear()
            ConcurrentIterator.Clear()
            ConcurrentMarcher.Clear()
        End Sub
    End Class
End Namespace