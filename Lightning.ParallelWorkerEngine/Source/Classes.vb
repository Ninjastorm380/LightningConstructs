Namespace Lightning
    Public Partial Class ParallelWorkerEngine(Of Worker As {ParallelWorker})
        Public MustInherit Class ParallelWorker
            Public MustOverride ReadOnly Property Completed As Boolean
            Public MustOverride Sub Run()
        End Class
    End Class
End Namespace