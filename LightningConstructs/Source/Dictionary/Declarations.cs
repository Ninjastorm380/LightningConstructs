

namespace Lightning
{
    public partial class Dictionary<TKey, TValue>
    {
        private TKey[] BaseKeysArray = new TKey[32768];
        private TValue[] BaseValuesArray = new TValue[32768];
        private bool[] BaseClaimedBitmap = new bool[32768];
        private System.Int32 FreePointer = 0;
        private readonly System.Collections.Generic.Queue<System.Int32> ReclaimedPointers = new System.Collections.Generic.Queue<System.Int32>();
        private System.Int32 ReclaimedCount = 0;
    }
}