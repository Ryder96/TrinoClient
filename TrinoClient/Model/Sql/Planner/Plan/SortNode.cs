using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SortNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.SortNode.java
                                 /// </summary>
    public class SortNode(PlanNodeId id, PlanNode source, OrderingScheme orderingScheme) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public OrderingScheme OrderingScheme { get; } = orderingScheme ?? throw new ArgumentNullException(nameof(orderingScheme));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
