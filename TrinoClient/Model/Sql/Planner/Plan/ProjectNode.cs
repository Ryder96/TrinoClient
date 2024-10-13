using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ProjectNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.ProjectNode.java
                                 /// </summary>
    public class ProjectNode(PlanNodeId id, PlanNode source, Assignments assignments) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public Assignments Assignments { get; } = assignments ?? throw new ArgumentNullException(nameof(assignments));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Assignments.GetOutputs();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
