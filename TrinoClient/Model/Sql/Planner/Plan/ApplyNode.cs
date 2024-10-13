using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Sql.Tree;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ApplyNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.ApplyNode.java
                                 /// </summary>
    public class ApplyNode(PlanNodeId id, PlanNode input, PlanNode subquery, Assignments subqueryAssignments, IEnumerable<Symbol> correlation, Node originSubquery) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Input { get; } = input ?? throw new ArgumentNullException(nameof(input));

        public PlanNode SubQuery { get; } = subquery ?? throw new ArgumentNullException(nameof(subquery));

        public IEnumerable<Symbol> Correlation { get; } = correlation ?? throw new ArgumentNullException(nameof(correlation));

        public Assignments SubqueryAssignments { get; } = subqueryAssignments ?? throw new ArgumentNullException(nameof(subqueryAssignments));

        public Node OriginSubquery { get; } = originSubquery ?? throw new ArgumentNullException(nameof(originSubquery));

        public IEnumerable<Symbol> OutputSymbols
        {
            get
            {
                return Input.GetOutputSymbols().Concat(SubqueryAssignments.GetOutputs());
            }
        }

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Input;

            yield return SubQuery;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Input.GetOutputSymbols().Concat(SubqueryAssignments.GetOutputs());
        }

        #endregion
    }
}
