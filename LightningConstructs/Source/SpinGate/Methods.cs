namespace Lightning
{
    public partial class SpinGate<T>
    {
        public SpinGate(System.Double Rate = 20.0)
        {
            BaseRate = Rate;
        }

        public T Lock(System.Int32 TimeoutMS = 0)
        {
            BaseTimeoutLimit = TimeoutMS;
            if (BaseReturnCalledEarlyFlag == false)
            {
                BaseReturnFlag = false;
                do
                {
                    if (BaseTimeoutLimit > 0.0)
                    {
                        BaseTimeoutCounter += SyncGovernor.IterationElapsed;
                        if (BaseTimeoutCounter >= BaseTimeoutLimit)
                        {
                            throw new System.TimeoutException("Gate timed out.");
                        } 
                    }
                    SyncGovernor.Limit();
                }
                while (BaseReturnFlag == false);
            }
            BaseReturnCalledEarlyFlag = false;
            BaseReturnFlag = false;
            return BaseReturnObject;
        }
        public void Unlock(T ReturnValue)
        {
            BaseReturnObject = ReturnValue;
            BaseReturnCalledEarlyFlag = true;
            BaseReturnFlag = true;
        }
    }
}