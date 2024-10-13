using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.JoinNode.java
    /// </summary>
    public class JoinNode : PlanNode
    {
        #region Public Properties

        public JoinType Type { get; }

        public PlanNode Left { get; }

        public PlanNode Right { get; }

        public IEnumerable<EquiJoinClause> Criteria { get; }

        /**
         * List of output symbols produced by join. Output symbols
         * must be from either left or right side of join. Symbols
         * from left join side must precede symbols from right side
         * of join.
         */
        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public dynamic Filter { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol LeftHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol RightHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public DistributionType DistributionType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public bool SpatialJoin { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public JoinNode(
            PlanNodeId id,
            JoinType type,
            PlanNode left,
            PlanNode right,
            IEnumerable<EquiJoinClause> criteria,
            IEnumerable<Symbol> outputSymbols,
            dynamic filter,
            Symbol leftHashSymbol,
            Symbol rightHashSymbol,
            DistributionType distributionType
            ) : base(id)
        {
            Type = type;
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            OutputSymbols = outputSymbols ?? throw new ArgumentNullException(nameof(outputSymbols));
            Filter = filter;
            LeftHashSymbol = leftHashSymbol;
            RightHashSymbol = rightHashSymbol;
            DistributionType = distributionType;

            HashSet<Symbol> InputSymbols = new(Left.GetOutputSymbols().Concat(Right.GetOutputSymbols()));

            ParameterCheck.Check(OutputSymbols.All(x => InputSymbols.Contains(x)), "Left and right join inputs do not contain all output symbols.");

            ParameterCheck.Check(!IsCrossJoin() || InputSymbols.Equals(OutputSymbols), "Cross join does not support output symbols pruning or reordering.");

            ParameterCheck.Check(!(!Criteria.Any() && LeftHashSymbol != null), "Left hash symbol is only valid in equijoin.");
            ParameterCheck.Check(!(!Criteria.Any() && RightHashSymbol != null), "Right hash symbol is only valid in equijoin.");
        }

        #endregion

        #region Public Methods

        public bool IsCrossJoin()
        {
            // Criteria is empty and no filter and join type is inner then it is a cross join
            return !Criteria.Any() && Filter == null && Type == JoinType.INNER;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Left;

            yield return Right;
        }

        #endregion
    }
}
