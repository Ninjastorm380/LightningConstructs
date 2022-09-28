namespace Lightning
{
    public partial class QueueStream<T>
    {
        public QueueStream(System.Int32 Capacity = 65536)
        {
            BaseBuffer = new T[Capacity];
            System.Array.Resize(ref BaseBuffer, Capacity);
        }

        public void Read(ref T[] Buffer, System.Int32 Offset, System.Int32 Seek, System.Int32 Count)
        {
            BlockCopy(ref BaseBuffer, Seek, ref Buffer, Offset, Count);
        }

        public void Shift(System.Int32 Amount)
        {
            if (WritePointer - Amount > 0) { BlockCopy(ref BaseBuffer, Amount, ref BaseBuffer, 0, WritePointer - Amount); }
            WritePointer = WritePointer - Amount;
        }

        public void Write(ref T[] Buffer, System.Int32 Offset, System.Int32 Count)
        {
            if (BaseBuffer.Length < WritePointer + Count) { System.Array.Resize(ref BaseBuffer, WritePointer + Count); }
            BlockCopy(ref Buffer, Offset, ref BaseBuffer, WritePointer, Count);
            WritePointer = WritePointer + Count;
        }

        private void BlockCopy(ref T[] InputBuffer, System.Int32 InputBufferOffset, ref T[] OutputBuffer, System.Int32 OutputBufferOffset, System.Int32 Count)
        {
            for (System.Int32 Index = InputBufferOffset; Index <= InputBufferOffset + Count - 1; Index++) 
            { OutputBuffer[(Index - InputBufferOffset) + OutputBufferOffset] = InputBuffer[Index]; }
        }
    }
}