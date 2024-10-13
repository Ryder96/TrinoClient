using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Sql.Planner;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageInfo.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.execution.StageInfo.java
                                 /// </summary>
    public class StageInfo(
        StageId stageId,
        StageState state,
        Uri self,
        PlanFragment plan,
        IEnumerable<string> types,
        StageStats stageStats,
        IEnumerable<TaskInfo> tasks,
        IEnumerable<StageInfo> subStages,
        ExecutionFailureInfo failureCause
            )
    {
        #region Public Properties

        public StageId StageId { get; } = stageId ?? throw new ArgumentNullException(nameof(stageId));

        public StageState State { get; } = state;

        public Uri Self { get; } = self ?? throw new ArgumentNullException(nameof(self));

        public PlanFragment Plan { get; } = plan;

        /// <summary>
        /// Should be IType interface
        /// </summary>
        public IEnumerable<string> Types { get; } = types;

        public StageStats StageStats { get; } = stageStats ?? throw new ArgumentNullException(nameof(stageStats));

        public IEnumerable<TaskInfo> Tasks { get; } = tasks ?? throw new ArgumentNullException(nameof(tasks));

        public IEnumerable<StageInfo> SubStages { get; } = subStages ?? throw new ArgumentNullException(nameof(subStages));

        public ExecutionFailureInfo FailureCause { get; } = failureCause;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("stageId", StageId)
                .Add("state", State.ToString())
                .ToString();
        }

        public bool IsCompleteInfo()
        {
            return State.IsDone() && Tasks.All(x => x.Complete);
        }

        #endregion
    }
}
