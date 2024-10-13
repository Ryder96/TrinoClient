using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskStatus.java
    /// </summary>
    public class TaskStatus
    {

        #region Private Fields

        /// <summary>
        /// The first valid version that will be returned for a remote task.
        /// </summary>
        public static readonly long STARTING_VERSION = 1;

        /// <summary>
        /// A value lower than {@link #STARTING_VERSION}. This value can be used to
        /// create an initial local task that is always older than any remote task.
        /// </summary>
        private static readonly long MIN_VERSION = 0;

        /// <summary>
        /// A value larger than any valid value. This value can be used to create
        /// a final local task that is always newer than any remote task.
        /// </summary>
        private static readonly long MAX_VERSION = long.MaxValue;

        #endregion

        #region Public Properties

        public TaskId TaskId { get; }

        public Guid TaskInstanceId { get; }

        public long Version { get; }

        public TaskState State { get; }

        public Uri Self { get; }

        public Guid NodeId { get; }

        public HashSet<Lifespan> CompletedDriverGroups { get; }

        public IEnumerable<ExecutionFailureInfo> Failures { get; }

        public int QueuedPartitionedDrivers { get; }

        public int RunningPartitionedDrivers { get; }

        public bool OutputBufferOverutilized { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public DataSize MemoryReservation { get; }

        public DataSize SystemMemoryReservation { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TaskStatus(
            TaskId taskId,
            Guid taskInstanceId,
            long version,
            TaskState state,
            Uri self,
            Guid nodeId,
            HashSet<Lifespan> completedDriverGroups,
            IEnumerable<ExecutionFailureInfo> failures,
            int queuedPartitionDrivers,
            int runningPartitionDrivers,
            bool outputBufferOverutilized,
            DataSize physicalWrittenDataSize,
            DataSize memoryReservation,
            DataSize systemMemoryReservation
            )
        {
            ParameterCheck.OutOfRange(version >= MIN_VERSION, "version", $"The version cannot be less than the minimum version of {MIN_VERSION}.");
            ParameterCheck.OutOfRange(queuedPartitionDrivers >= 0, "queuedPartitionDrivers", "The queued partition drivers cannot be less than 0.");
            ParameterCheck.OutOfRange(runningPartitionDrivers >= 0, "runningPartitionDrivers", "The running partition drivers cannot be less than 0.");

            TaskId = taskId ?? throw new ArgumentNullException(nameof(taskId));
            TaskInstanceId = taskInstanceId;
            Version = version;
            State = state;
            Self = self ?? throw new ArgumentNullException(nameof(self));
            NodeId = nodeId;
            CompletedDriverGroups = completedDriverGroups ?? throw new ArgumentNullException(nameof(completedDriverGroups));

            QueuedPartitionedDrivers = queuedPartitionDrivers;
            RunningPartitionedDrivers = runningPartitionDrivers;
            OutputBufferOverutilized = outputBufferOverutilized;
            PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException(nameof(physicalWrittenDataSize));
            MemoryReservation = memoryReservation ?? throw new ArgumentNullException(nameof(memoryReservation));
            SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException(nameof(systemMemoryReservation));
            Failures = failures ?? throw new ArgumentNullException(nameof(failures));
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("taskId", TaskId)
                .Add("state", State)
                .ToString();
        }

        public static TaskStatus InitialTaskStatus(TaskId taskId, Uri location, string nodeId)
        {
            return new TaskStatus(
                taskId,
                Guid.Empty,
                MIN_VERSION,
                TaskState.PLANNED,
                location,
                 Guid.Parse(nodeId),
                 [],
                 [],
                 0,
                 0,
                 false,
                 new DataSize(0, DataSizeUnit.BYTE),
                 new DataSize(0, DataSizeUnit.BYTE),
                 new DataSize(0, DataSizeUnit.BYTE)
            );
        }

        public static TaskStatus FailWith(TaskStatus taskStatus, TaskState state, List<ExecutionFailureInfo> exceptions)
        {
            return new TaskStatus(
                taskStatus.TaskId,
                taskStatus.TaskInstanceId,
                MAX_VERSION,
                state,
                taskStatus.Self,
                taskStatus.NodeId,
                taskStatus.CompletedDriverGroups,
                exceptions,
                taskStatus.QueuedPartitionedDrivers,
                taskStatus.RunningPartitionedDrivers,
                taskStatus.OutputBufferOverutilized,
                taskStatus.PhysicalWrittenDataSize,
                taskStatus.MemoryReservation,
                taskStatus.SystemMemoryReservation
            );
        }

        #endregion
    }
}
