    Public Partial Class SpinGate(Of T)
        Public Sub New(ByVal Optional Rate As System.Double = 20.0)
            BaseRate = Rate
        End Sub

        Public Function Lock(ByVal Optional TimeoutMS As System.Int32 = 0) As T
            BaseTimeoutLimit = TimeoutMS

            If BaseReturnCalledEarlyFlag = False Then
                BaseReturnFlag = False

                Do

                    If BaseTimeoutLimit > 0.0 Then
                        BaseTimeoutCounter += SyncGovernor.IterationElapsed

                        If BaseTimeoutCounter >= BaseTimeoutLimit Then
                            Throw New System.TimeoutException("SpinGate timed out!")
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