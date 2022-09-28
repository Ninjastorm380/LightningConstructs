
namespace Lightning
{
    public partial class Governor
    {
        public Governor(System.Double Rate = 100.0)
        {
            BaseRate = Rate;
            BaseSleepOffsetConstant = System.TimeSpan.TicksPerMillisecond / 10;
            BaseTimeConstant = System.TimeSpan.TicksPerMillisecond * (System.Int64)(1000.0 / BaseRate);
            BaseSleepTarget = new System.TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant);
            BaseGovernorWatch = System.Diagnostics.Stopwatch.StartNew();
        }

        public void Limit()
        {
            if (BaseSleepTarget.Ticks > 1000) { System.Threading.Thread.Sleep(BaseSleepTarget); }
            do { } while (BaseGovernorWatch.ElapsedTicks < BaseTimeConstant);
            BaseDelta = BaseGovernorWatch.ElapsedTicks; BaseGovernorWatch.Restart();
        }
    }
}