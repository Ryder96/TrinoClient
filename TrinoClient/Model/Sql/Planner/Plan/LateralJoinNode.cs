using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Sql.Tree;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.LateralJoinNode.java
    /// </summary>
    public class LateralJoinNode : PlanNode
    {
        #region Public Properties

        public PlanNode Input { get; }

        public PlanNode Subquery { get; }

        /// <summary>
        /// Correlation symbols, returned from input (outer plan) used in subquery (inner plan)
        /// </summary>
        public IEnumerable<Symbol> Correlation { get; }

        public LateralJoinType Type { get; }

        /// <summary>
        /// Used for error reporting in case this ApplyNode is not supported
        /// </summary>
        public Node OriginSubquery { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public LateralJoinNode(PlanNodeId id, PlanNode input, PlanNode subquery, IEnumerable<Symbol> correlation, Node originSubquery) : base(id)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Subquery = subquery ?? throw new ArgumentNullException(nameof(subquery));
            Correlation = correlation ?? throw new ArgumentNullException(nameof(correlation));
            OriginSubquery = originSubquery ?? throw new ArgumentNullException(nameof(originSubquery));

            ParameterCheck.Check(Correlation.All(x => Input.GetOutputSymbols().Contains(x)), "Input does not contain symbol from correlation.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Input.GetOutputSymbols().Concat(Subquery.GetOutputSymbols());
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Input;

            yield return Subquery;
        }

        #endregion
    }
}
