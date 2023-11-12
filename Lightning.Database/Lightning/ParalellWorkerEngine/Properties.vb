Namespace Lightning
    Public Partial Class ParallelWorkerEngine(Of Worker As {ParallelWorker})
        Public ReadOnly Property NextCompleted As Boolean
            Get 
                Dim PeekedWorker As Worker = ConcurrentMarcher.Peek()
                
                If PeekedWorker IsNot Nothing Then Return PeekedWorker.Completed Else Return False
            End Get
        End Property
        
        Public ReadOnly Property Count As Int32
            Get 
                Return ConcurrentMarcher.Count
            End Get
        End Property

    End Class
End Namespace