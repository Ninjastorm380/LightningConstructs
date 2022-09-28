namespace Lightning
{
    public partial class SpinGate<T>
    {
        private System.Object LockObject = new System.Object();
        private T BaseReturnObject = default!;
        private System.Boolean BaseReturnFlag = false;
        private System.Boolean BaseReturnCalledEarlyFlag = false;
        private System.Double BaseRate = 20.0;
        private System.Double BaseTimeoutCounter = 0.0;
        private System.Double BaseTimeoutLimit = 0.0;
        private Governor SyncGovernor = new Governor(20.0);
    }
}