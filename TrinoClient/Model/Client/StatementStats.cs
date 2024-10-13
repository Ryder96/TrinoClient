using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.StatementStats.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.client.StatementStats.java
                                 /// </summary>
    public class StatementStats(
        string state,
        bool queued,
        bool scheduled,
        int nodes,
        int totalSplits,
        int queuedSplits,
        int runningSplits,
        int completedSplits,
        long userTimeMillis,
        long cpuTimeMillis,
        long wallTimeMillis,
        long queuedTimeMillis,
        long elapsedTimeMillis,
        long processedRows,
        long processedBytes,
        long peakMemoryBytes,
        StageStats rootStage
        )
    {
        #region Public Properties

        public StatementState State { get; } = (StatementState)Enum.Parse(typeof(StatementState), state);

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Queued { get; } = queued;

        public bool Scheduled { get; } = scheduled;

        public int Nodes { get; } = nodes;

        public int TotalSplits { get; } = totalSplits;

        public int QueuedSplits { get; } = queuedSplits;

        public int RunningSplits { get; } = runningSplits;

        public int CompletedSplits { get; } = completedSplits;

        public long UserTimeMillis { get; } = userTimeMillis;

        public long CpuTimeMillis { get; } = cpuTimeMillis;

        public long WallTimeMillis { get; } = wallTimeMillis;

        public long QueuedTimeMillis { get; } = queuedTimeMillis;

        public long ElapsedTimeMillis { get; } = elapsedTimeMillis;

        public long ProcessedRows { get; } = processedRows;

        public long ProcessedBytes { get; } = processedBytes;

        public long PeakMemoryBytes { get; } = peakMemoryBytes;

        public StageStats RootStage { get; } = rootStage;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage
        {
            get
            {
                if (!Scheduled || TotalSplits == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Min(100, CompletedSplits * 100.0 / TotalSplits);
                }
            }
        }

        #endregion
        #region Constructor

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("state", State)
                .Add("queued", Queued)
                .Add("scheduled", Scheduled)
                .Add("nodes", Nodes)
                .Add("totalSplits", TotalSplits)
                .Add("queuedSplits", QueuedSplits)
                .Add("runningSplits", RunningSplits)
                .Add("completedSplits", CompletedSplits)
                .Add("userTimeMillis", UserTimeMillis)
                .Add("cpuTimeMillis", CpuTimeMillis)
                .Add("wallTimeMillis", WallTimeMillis)
                .Add("queuedTimeMillis", QueuedTimeMillis)
                .Add("elapsedTimeMillis", ElapsedTimeMillis)
                .Add("processedRows", ProcessedRows)
                .Add("processedBytes", ProcessedBytes)
                .Add("peakMemoryBytes", PeakMemoryBytes)
                .Add("rootStage", RootStage)
                .ToString();
        }

        #endregion
    }
}
