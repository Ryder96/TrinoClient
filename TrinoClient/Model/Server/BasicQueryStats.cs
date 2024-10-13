using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Execution;
using TrinoClient.Model.Execution.Scheduler;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Server
{
    /// <summary>
    /// Lightweight version of QueryStats. Parts of the web UI depend on the fields
    /// being named consistently across these classes.
    /// 
    /// From com.facebook.presto.server.BasicQueryStats.java
    /// </summary>
    public class BasicQueryStats
    {
        public DateTime CreateTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExecutionTime { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int RunningDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeUserMemory { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize PeakMemoryReservation { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; }

        public bool FullyBlocked { get; }

        public HashSet<BlockedReason> BlockedReasons { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage { get; }

        #region Constructors

        [JsonConstructor]
        public BasicQueryStats(
            DateTime createTime,
            DateTime endTime,
            TimeSpan elapsedTime,
            TimeSpan executionTime,
            int totalDrivers,
            int queuedDrivers,
            int runningDrivers,
            int completedDrivers,
            double cumulativeUserMemory,
            DataSize userMemoryReservation,
            DataSize peakUserMemoryReservation,
            TimeSpan totalCpuTime,
            bool fullyBlocked,
            HashSet<BlockedReason> blockedReasons,
            double progressPercentage
            )
        {
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers", "Total drivers cannot be negative.");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers", "Queued drivers cannot be negative.");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers", "Running drivers cannot be negative.");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers", "Completed drivers cannot be negative.");

            CreateTime = createTime;
            EndTime = endTime;

            ElapsedTime = elapsedTime;

            TotalDrivers = totalDrivers;
            QueuedDrivers = queuedDrivers;
            RunningDrivers = runningDrivers;
            CompletedDrivers = completedDrivers;

            CumulativeUserMemory = cumulativeUserMemory;
            UserMemoryReservation = userMemoryReservation;
            PeakMemoryReservation = peakUserMemoryReservation;
            TotalCpuTime = totalCpuTime;

            FullyBlocked = fullyBlocked;
            BlockedReasons = blockedReasons ?? throw new ArgumentNullException(nameof(blockedReasons));
            ProgressPercentage = progressPercentage;
        }

        public BasicQueryStats(QueryStats queryStats) : this(
            queryStats.CreateTime,
            queryStats.EndTime,
            queryStats.ElapsedTime,
            queryStats.ExecutionTime,
            queryStats.TotalDrivers,
            queryStats.QueuedDrivers,
            queryStats.RunningDrivers,
            queryStats.CompletedDrivers,
            queryStats.CumulativeUserMemory,
            queryStats.UserMemoryReservation,
            queryStats.PeakUserMemoryReservation,
            queryStats.TotalCpuTime,
            queryStats.FullyBlocked,
            queryStats.BlockedReasons,
            queryStats.ProgressPercentage
            )
        { }

        #endregion
    }
}
