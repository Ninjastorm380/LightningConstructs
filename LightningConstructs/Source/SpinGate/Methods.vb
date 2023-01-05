    Public Partial Class SpinGate(Of T)
        Public Sub New(ByVal Optional Rate As System.Double = 20.0)
            BaseRate = Rate
            SyncGovernor.Rate = BaseRate
        End Sub

        Public Function Lock(ByVal Optional TimeoutMS As Int32 = 0) As T
            BaseTimeoutLimit = TimeoutMS
            BaseTimeoutCounter = 0
            If BaseReturnCalledEarlyFlag = False Then
                BaseReturnFlag = False
                Do
                    If BaseTimeoutLimit > 0.0 Then
                        BaseTimeoutCounter += SyncGovernor.Elapsed

                        If BaseTimeoutCounter >= BaseTimeoutLimit Then
                            Throw New TimeoutException("SpinGate timed out!")
                        End If
                    End If
                    SyncGovernor.Limit()
                Loop While BaseReturnFlag = False
            End If

            BaseReturnCalledEarlyFlag = False
            BaseReturnFlag = False
            Return BaseReturnObject
        End Function

        Public Sub Unlock(ByVal ReturnValue As T)
            BaseReturnObject = ReturnValue
            BaseReturnCalledEarlyFlag = True
            BaseReturnFlag = True
        End Sub
    End Class
