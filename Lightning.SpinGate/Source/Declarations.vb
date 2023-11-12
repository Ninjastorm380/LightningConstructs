Namespace Lightning
    Public Partial Class SpinGate
        Private LockFlag As Boolean
        Private LockObject As Object
    End Class

    Public Partial Class SpinGate(Of T)
        Private LockFlag As Boolean
        Private LockObject As Object
        Private Result As T
    End Class
End Namespace