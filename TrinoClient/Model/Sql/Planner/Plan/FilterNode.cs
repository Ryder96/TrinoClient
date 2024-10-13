using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.FilterNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.FilterNode.java
                                 /// </summary>
    public class FilterNode(PlanNodeId id, PlanNode source, dynamic predicate) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source;

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Predicate { get; } = predicate;

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
