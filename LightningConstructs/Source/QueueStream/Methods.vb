    Public Partial Class QueueStream(Of T)
        Public Sub New(ByVal Optional Capacity As System.Int32 = 65536)
            BaseBuffer = New T(Capacity - 1) {}
            System.Array.Resize(BaseBuffer, Capacity)
        End Sub

        Public Sub Read(ByRef Buffer As T(), ByVal Offset As System.Int32, ByVal Seek As System.Int32, ByVal Count As System.Int32)
            BlockCopy(BaseBuffer, Seek, Buffer, Offset, Count)
        End Sub

        Public Sub Shift(ByVal Amount As System.Int32)
            If WritePointer - Amount > 0 Then
                BlockCopy(BaseBuffer, Amount, BaseBuffer, 0, WritePointer - Amount)
            End If

            WritePointer = WritePointer - Amount
        End Sub

        Public Sub Write(ByRef Buffer As T(), ByVal Offset As System.Int32, ByVal Count As System.Int32)
            If BaseBuffer.Length < WritePointer + Count Then
                System.Array.Resize(BaseBuffer, WritePointer + Count)
            End If

            BlockCopy(Buffer, Offset, BaseBuffer, WritePointer, Count)
            WritePointer = WritePointer + Count
        End Sub

        Private Sub BlockCopy(ByRef InputBuffer As T(), ByVal InputBufferOffset As System.Int32, ByRef OutputBuffer As T(), ByVal OutputBufferOffset As System.Int32, ByVal Count As System.Int32)
            For Index As System.Int32 = InputBufferOffset To InputBufferOffset + Count - 1
                OutputBuffer((Index - InputBufferOffset) + OutputBufferOffset) = InputBuffer(Index)
            Next
        End Sub
    End Class