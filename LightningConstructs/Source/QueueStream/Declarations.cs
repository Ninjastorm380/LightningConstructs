namespace Lightning
{
    public partial class QueueStream<T>
    {
        private T[] BaseBuffer = new T[65536];
        private System.Int32 WritePointer = 0;
    }
}