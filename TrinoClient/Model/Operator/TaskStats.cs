using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Execution.Scheduler;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.TaskStats.java
    /// </summary>
    public class TaskStats
    {
        #region Public Properties

        public DateTime CreateTime { get; }

        public DateTime FirstStartTime { get; }

        public DateTime LastStartTime { get; }

        public DateTime LastEndTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan QueuedTime { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int QueuedPartitionedDrivers { get; }

        public int RunningDrivers { get; }

        public int RunningPartitionedDrivers { get; }

        public int BlockedDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeUserMemory { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize RevocableMemoryReservation { get; }

        public DataSize SystemMemoryReservation { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalScheduledTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalUserTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalBlockedTime { get; }

        public bool FullyBlocked { get; }

        public HashSet<BlockedReason> BlockedReasons { get; }

        public DataSize RawInputDataSize { get; }

        public long RawInputPositions { get; }

        public DataSize ProcessedInputDataSize { get; }

        public long ProcessedInputPositions { get; }

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public IEnumerable<PipelineStats> Pipelines { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TaskStats(
            DateTime createTime,
            DateTime firstStartTime,
            DateTime lastStartTime,
            DateTime lastEndTime,
            DateTime endTime,
            TimeSpan elapsedTime,
            TimeSpan queuedTime,

            int totalDrivers,
            int queuedDrivers,
            int queuedPartitionedDrivers,
            int runningDrivers,
            int runningPartitionedDrivers,
            int blockedDrivers,
            int completedDrivers,

            double cumulativeUserMemory,
            DataSize userMemoryReservation,
            DataSize revocableMemoryReservation,
            DataSize systemMemoryReservation,

            TimeSpan totalScheduledTime,
            TimeSpan totalCpuTime,
            TimeSpan totalUserTime,
            TimeSpan totalBlockedTime,
            bool fullyBlocked,
            HashSet<BlockedReason> blockedReasons,

            DataSize rawInputDataSize,
            long rawInputPositions,

            DataSize processedInputDataSize,
            long processedInputPositions,

            DataSize outputDataSize,
            long outputPositions,

            DataSize physicalWrittenDataSize,

            IEnumerable<PipelineStats> pipelines
        )
        {
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers");
            ParameterCheck.OutOfRange(queuedPartitionedDrivers >= 0, "queuedPartitionedDrivers");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers");
            ParameterCheck.OutOfRange(runningPartitionedDrivers >= 0, "runningPartitionedDrivers");
            ParameterCheck.OutOfRange(blockedDrivers >= 0, "blockedDrivers ");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers");
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions");

            CreateTime = createTime;
            FirstStartTime = firstStartTime;
            LastStartTime = lastStartTime;
            LastEndTime = lastEndTime;
            EndTime = endTime;
            ElapsedTime = elapsedTime;
            QueuedTime = queuedTime;


            TotalDrivers = totalDrivers;
            QueuedDrivers = queuedDrivers;
            QueuedPartitionedDrivers = queuedPartitionedDrivers;

            RunningDrivers = runningDrivers;
            RunningPartitionedDrivers = runningPartitionedDrivers;
            BlockedDrivers = blockedDrivers;
            CompletedDrivers = completedDrivers;

            CumulativeUserMemory = cumulativeUserMemory;
            UserMemoryReservation = userMemoryReservation ?? throw new ArgumentNullException(nameof(userMemoryReservation));
            RevocableMemoryReservation = revocableMemoryReservation ?? throw new ArgumentNullException(nameof(revocableMemoryReservation));
            SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException(nameof(systemMemoryReservation));

            TotalScheduledTime = totalScheduledTime;
            TotalCpuTime = totalCpuTime;
            TotalUserTime = totalUserTime;
            TotalBlockedTime = totalBlockedTime;
            FullyBlocked = fullyBlocked;
            BlockedReasons = blockedReasons ?? throw new ArgumentNullException(nameof(blockedReasons));

            RawInputDataSize = rawInputDataSize ?? throw new ArgumentNullException(nameof(rawInputDataSize));
            RawInputPositions = rawInputPositions;

            ProcessedInputDataSize = processedInputDataSize ?? throw new ArgumentNullException(nameof(processedInputDataSize));
            ProcessedInputPositions = processedInputPositions;

            OutputDataSize = outputDataSize ?? throw new ArgumentNullException(nameof(outputDataSize));
            OutputPositions = outputPositions;

            PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException(nameof(outputDataSize));

            Pipelines = pipelines ?? throw new ArgumentNullException(nameof(pipelines));
        }

        #endregion
    }
}
