using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.RemoteSourceNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.RemoteSourceNode.java
                                 /// </summary>
    public class RemoteSourceNode(PlanNodeId id, IEnumerable<PlanFragmentId> sourceFragmentIds, IEnumerable<Symbol> outputs) : PlanNode(id)
    {
        #region Public Properties

        public IEnumerable<PlanFragmentId> SourceFragmentIds { get; } = sourceFragmentIds;

        public IEnumerable<Symbol> Outputs { get; } = outputs ?? throw new ArgumentNullException(nameof(outputs));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return [];
        }

        #endregion
    }
}
