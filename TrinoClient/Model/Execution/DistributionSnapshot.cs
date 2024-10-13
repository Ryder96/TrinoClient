using Newtonsoft.Json;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From io.airlift.stats.Distribution.java (internal class DistributionSnapshot)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From io.airlift.stats.Distribution.java (internal class DistributionSnapshot)
                                 /// </summary>
    public class DistributionSnapshot(
        double maxError,
        double count,
        double total,
        long p01,
        long p05,
        long p10,
        long p25,
        long p50,
        long p75,
        long p90,
        long p95,
        long p99,
        long min,
        long max
            )
    {
        #region Public Properties

        public double MaxError { get; } = maxError;

        public double Count { get; } = count;

        public double Total { get; } = total;

        public long P01 { get; } = p01;

        public long P05 { get; } = p05;

        public long P10 { get; } = p10;

        public long P25 { get; } = p25;

        public long P50 { get; } = p50;

        public long P75 { get; } = p75;

        public long P90 { get; } = p90;

        public long P95 { get; } = p95;

        public long P99 { get; } = p99;

        public long Min { get; } = min;

        public long Max { get; } = max;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxError", MaxError)
                .Add("count", Count)
                .Add("total", Total)
                .Add("p01", P01)
                .Add("p05", P05)
                .Add("p10", P10)
                .Add("p25", P25)
                .Add("p50", P50)
                .Add("p75", P75)
                .Add("p90", P90)
                .Add("p95", P95)
                .Add("p99", P99)
                .Add("min", Min)
                .Add("mix", Max)
                .ToString();
        }

        #endregion
    }
}
