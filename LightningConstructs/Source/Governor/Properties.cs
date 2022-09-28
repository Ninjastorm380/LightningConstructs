namespace Lightning
{
    public partial class Governor
    {
        public System.Double Rate
        {
            get => BaseRate;
            set
            {
                BaseRate = value;
                BaseTimeConstant = System.TimeSpan.TicksPerMillisecond * (System.Int64)(1000.0 / BaseRate);
                BaseSleepTarget = new System.TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant);
            }
        }
        public System.Double Delta => BaseDelta / (System.Double)BaseTimeConstant;
        public System.Double DeltaInverse => (System.Double)BaseTimeConstant / BaseDelta;
        public System.Double IterationElapsed =>  (1000.0 / BaseDelta);
        public System.Double IterationProjected => (1000.0 / BaseRate);
    }
}