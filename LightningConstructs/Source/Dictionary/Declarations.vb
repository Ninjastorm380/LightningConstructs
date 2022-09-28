    Public Partial Class Dictionary(Of TKey, TValue)
        Private BaseKeysArray As TKey() = New TKey(32767) {}
        Private BaseValuesArray As TValue() = New TValue(32767) {}
        Private BaseClaimedBitmap As Boolean() = New Boolean(32767) {}
        Private FreePointer As System.Int32 = 0
        Private ReadOnly ReclaimedPointers As System.Collections.Generic.Queue(Of System.Int32) = New System.Collections.Generic.Queue(Of System.Int32)()
        Private ReclaimedCount As System.Int32 = 0
    End Class