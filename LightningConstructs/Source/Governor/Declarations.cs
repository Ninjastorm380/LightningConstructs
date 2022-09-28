namespace Lightning
{
    public partial class Governor
    {
        private System.Double BaseRate;
        private System.Int64 BaseTimeConstant;
        private System.Int64 BaseDelta;

        private readonly System.Diagnostics.Stopwatch BaseGovernorWatch;
        private readonly System.Int64 BaseSleepOffsetConstant;
        private System.TimeSpan BaseSleepTarget;
    }
}