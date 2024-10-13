using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Execution.Buffer;
using TrinoClient.Model.Operator;
using TrinoClient.Model.Sql.Planner.Plan;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskInfo.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.execution.TaskInfo.java
                                 /// </summary>
    public class TaskInfo(
        TaskStatus taskStatus,
        DateTime lastHeartbeat,
        OutputBufferInfo outputBuffers,
        HashSet<PlanNodeId> noMoreSplits,
        TaskStats stats,
        bool needsPlan,
        bool complete
            )
    {
        #region Public Properties

        public TaskStatus TaskStatus { get; } = taskStatus ?? throw new ArgumentNullException(nameof(taskStatus));

        public DateTime LastHeartbeat { get; } = lastHeartbeat;

        public OutputBufferInfo OutputBuffers { get; } = outputBuffers ?? throw new ArgumentNullException(nameof(outputBuffers));

        public HashSet<PlanNodeId> NoMoreSplits { get; } = noMoreSplits ?? throw new ArgumentNullException(nameof(noMoreSplits));

        public TaskStats Stats { get; } = stats ?? throw new ArgumentNullException(nameof(stats));

        public bool NeedsPlan { get; } = needsPlan;

        public bool Complete { get; } = complete;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("taskId", TaskStatus.TaskId)
                .Add("state", TaskStatus.State.ToString())
                .ToString();
        }

        public static TaskInfo CreateInitialTask(TaskId taskId, Uri location, string nodeId, IEnumerable<BufferInfo> bufferStates, TaskStats taskStats)
        {
            return new TaskInfo(
                    TaskStatus.InitialTaskStatus(taskId, location, nodeId),
                    DateTime.Now,
                    new OutputBufferInfo("UNINITIALIZED", BufferState.OPEN, true, true, 0, 0, 0, 0, bufferStates),
                    [],
                    taskStats,
                    true,
                    false
                );
        }

        public TaskInfo WithTaskStatus(TaskStatus newTaskStatus)
        {
            return new TaskInfo(newTaskStatus, LastHeartbeat, OutputBuffers, NoMoreSplits, Stats, NeedsPlan, Complete);
        }

        #endregion
    }
}
