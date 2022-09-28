    Public Partial Class Dictionary(Of TKey, TValue)
        Public ReadOnly Property Length As System.Int32
            Get
                Return FreePointer - ReclaimedCount
            End Get
        End Property

        Default Public Property Item(ByVal Key As TKey) As TValue
            Get
                Dim ClaimedPointer As System.Int32 = Search(Key)

                If ClaimedPointer = -1 Then
                    Throw New System.Collections.Generic.KeyNotFoundException($"The key was not found in the dictionary!")
                End If

                Return BaseValuesArray(ClaimedPointer)
            End Get
            Set(ByVal value As TValue)
                Dim ClaimedPointer As System.Int32 = Search(Key)

                If ClaimedPointer = -1 Then
                    Throw New System.Collections.Generic.KeyNotFoundException($"The key was not found in the dictionary!")
                End If

                BaseValuesArray(ClaimedPointer) = value
            End Set
        End Property
    End Class

