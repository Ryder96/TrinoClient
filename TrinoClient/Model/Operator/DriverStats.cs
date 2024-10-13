using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Execution.Scheduler;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.DriverStats.java
    /// </summary>
    public class DriverStats
    {
        #region Public Properties

        public DateTime CreateTime { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan QueuedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

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

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan RawInputReadTime { get; }

        public DataSize ProcessedInputDataSize { get; }

        public long ProcessedInputPositions { get; }

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public IEnumerable<OperatorStats> OperatorStats { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DriverStats(
            DateTime createTime,
            DateTime startTime,
            DateTime endTime,
            TimeSpan queuedTime,
            TimeSpan elapsedTime,

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
            TimeSpan rawInputReadTime,

            DataSize processedInputDataSize,
            long processedInputPositions,

            DataSize outputDataSize,
            long outputPositions,

            DataSize physicalWrittenDataSize,

            IEnumerable<OperatorStats> operatorStats
            )
        {
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions");

            CreateTime = createTime;
            StartTime = startTime;
            EndTime = endTime;
            QueuedTime = queuedTime;
            ElapsedTime = elapsedTime;

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
            RawInputReadTime = rawInputReadTime;

            ProcessedInputDataSize = processedInputDataSize ?? throw new ArgumentNullException(nameof(processedInputDataSize));
            ProcessedInputPositions = processedInputPositions;

            OutputDataSize = outputDataSize ?? throw new ArgumentNullException(nameof(outputDataSize));
            OutputPositions = outputPositions;

            PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException(nameof(physicalWrittenDataSize));

            OperatorStats = operatorStats ?? throw new ArgumentNullException(nameof(operatorStats));
        }

        #endregion
    }
}
