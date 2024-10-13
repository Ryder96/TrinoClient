using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.StageStats.java
    /// </summary>
    public class StageStats
    {
        #region Public Properties

        public string StageId { get; }

        public string State { get; }

        public bool Done { get; }

        public int Nodes { get; }

        public int TotalSplits { get; }

        public int QueuedSplits { get; }

        public int RunningSplits { get; }

        public int CompletedSplits { get; }

        public long UserTimeMillis { get; }

        public long CpuTimeMillis { get; }

        public long WallTimeMillis { get; }

        public long ProcessedRows { get; }

        public long ProcessedBytes { get; }

        public IEnumerable<StageStats> SubStages { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StageStats(
            string stageId,
            string state,
            bool done,
            int nodes,
            int totalSplits,
            int queuedSplits,
            int runningSplits,
            int completedSplits,
            long userTimeMillis,
            long cpuTimeMillis,
            long wallTimeMillis,
            long processedRows,
            long processedBytes,
            IEnumerable<StageStats> subStages
            )
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            StageId = stageId;
            State = state;
            Done = done;
            Nodes = nodes;
            TotalSplits = totalSplits;
            QueuedSplits = queuedSplits;
            RunningSplits = runningSplits;
            CompletedSplits = completedSplits;
            UserTimeMillis = userTimeMillis;
            CpuTimeMillis = cpuTimeMillis;
            WallTimeMillis = wallTimeMillis;
            ProcessedRows = processedRows;
            ProcessedBytes = processedBytes;
            SubStages = subStages ?? throw new ArgumentNullException(nameof(subStages));
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("state", State)
                .Add("done", Done)
                .Add("nodes", Nodes)
                .Add("totalSplits", TotalSplits)
                .Add("queuedSplits", QueuedSplits)
                .Add("runningSplits", RunningSplits)
                .Add("completedSplits", CompletedSplits)
                .Add("userTimeMillis", UserTimeMillis)
                .Add("cpuTimeMillis", CpuTimeMillis)
                .Add("wallTimeMillis", WallTimeMillis)
                .Add("processedRows", ProcessedRows)
                .Add("processedBytes", ProcessedBytes)
                .Add("subStages", SubStages)
                .ToString();
        }

        #endregion
    }
}
