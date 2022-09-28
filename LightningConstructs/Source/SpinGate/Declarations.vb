
    Public Partial Class SpinGate(Of T)
        Private LockObject As System.Object = New System.Object()
        Private BaseReturnObject As T = Nothing
        Private BaseReturnFlag As System.Boolean = False
        Private BaseReturnCalledEarlyFlag As System.Boolean = False
        Private BaseRate As System.Double = 20.0
        Private BaseTimeoutCounter As System.Double = 0.0
        Private BaseTimeoutLimit As System.Double = 0.0
        Private SyncGovernor As Governor = New Governor(20.0)
    End Class
