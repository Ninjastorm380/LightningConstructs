namespace Lightning
{
    public partial class Dictionary<TKey, TValue>
    {
        private System.Int32 Search(TKey Key)
        {
            if (FreePointer == 0)
                return -1;

            for (var Index = 0; Index <= FreePointer - 1; Index++)
            {
                if (BaseClaimedBitmap[Index] == true)
                {
                    if (BaseKeysArray[Index]?.Equals(Key) == true)
                        return Index;
                }
            }
            return -1;
        }
        public void Add(TKey Key, TValue Value)
        {
            if (Search(Key) == -1)
            {
                if (ReclaimedCount == 0)
                {
                    if (FreePointer >= BaseKeysArray.Length)
                    { 
                        System.Array.Resize(ref BaseKeysArray, FreePointer * 2);
                        System.Array.Resize(ref BaseValuesArray, FreePointer * 2);
                        System.Array.Resize(ref BaseClaimedBitmap, FreePointer * 2);
                    }

                    BaseKeysArray[FreePointer] = Key;
                    BaseValuesArray[FreePointer] = Value;
                    BaseClaimedBitmap[FreePointer] = true;
                    FreePointer += 1;
                }
                else
                {
                    var ClaimedPointer = ReclaimedPointers.Dequeue();
                    BaseKeysArray[ClaimedPointer] = Key;
                    BaseValuesArray[ClaimedPointer] = Value;
                    BaseClaimedBitmap[ClaimedPointer] = true;
                    ReclaimedCount -= 1;
                }
            }
        }
        public void Remove(TKey Key)
        {
            System.Int32 ClaimedPointer = Search(Key);
            if (ClaimedPointer != -1)
            {
                ReclaimedPointers.Enqueue(ClaimedPointer);
                BaseClaimedBitmap[ClaimedPointer] = false;
                ReclaimedCount += 1;
            }
        }
        public bool Contains(TKey Key)
        {
            return Search(Key) != -1;
        }
    }
}