using System.Collections.Generic;

namespace Lightning
{
    public partial class Dictionary<TKey, TValue>
    {
        public System.Int32 Length => FreePointer - ReclaimedCount;

        public TValue this[TKey Key]
        {
            get
            {
                System.Int32 ClaimedPointer = Search(Key);
                if (ClaimedPointer == -1) { throw new System.Collections.Generic.KeyNotFoundException($"Key {Key?.ToString()} was not found in the dictionary."); }
                return BaseValuesArray[ClaimedPointer];
            }
            set
            {
                System.Int32 ClaimedPointer = Search(Key);
                if (ClaimedPointer == -1) { throw new System.Collections.Generic.KeyNotFoundException($"Key {Key?.ToString()} was not found in the dictionary."); }
                    BaseValuesArray[ClaimedPointer] = value;
            }
        } 
    }
}