Namespace Lightning
    Public Partial Class ParallelWorkerEngine(Of Worker As {ParallelWorker})
        Private ConcurrentMarcher As Queue(Of Worker)
        Private ConcurrentIterator As List(Of Worker)
    End Class
End Namespace