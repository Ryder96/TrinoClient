using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.EnforceSingleRowNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.EnforceSingleRowNode.java
                                 /// </summary>
    public class EnforceSingleRowNode(PlanNodeId id, PlanNode source) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols();
        }

        #endregion
    }
}
