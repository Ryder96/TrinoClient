using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Metadata;
using TrinoClient.Model.Sql.Tree;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.AggregationNode.java
    /// </summary>
    public class AggregationNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        /// <summary>
        /// TODO: Should be IDictionary<Symbol, Aggregation> but symbol doesn't work as key
        /// </summary>
        public IDictionary<string, Aggregation> Aggregations { get; }

        public IEnumerable<List<Symbol>> GroupingSets { get; }

        public Step Step { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol GroupIdSymbol { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public AggregationNode(
            PlanNodeId id,
            PlanNode source,
            IDictionary<string, Aggregation> aggregations,
            IEnumerable<List<Symbol>> groupingSets,
            Step step,
            Symbol hashSymbol,
            Symbol groupIdSymbol) : base(id)
        {
            Source = source;
            Aggregations = aggregations;

            ArgumentNullException.ThrowIfNull(groupingSets);

            if (!groupingSets.Any())
            {
                throw new ArgumentException("Grouping sets cannot be empty.", nameof(groupingSets));
            }

            GroupingSets = groupingSets;
            Step = step;
            HashSymbol = hashSymbol;
            GroupIdSymbol = groupIdSymbol;

            Outputs = GetGroupingKeys().Concat(Aggregations.Keys.Select(x => new Symbol(x)));

            if (HashSymbol != null)
            {
                Outputs = Outputs.Concat([HashSymbol]);
            }
        }

        #endregion

        #region Public Methods

        public IEnumerable<Symbol> GetGroupingKeys()
        {
            IEnumerable<Symbol> Temp = GroupingSets.SelectMany(x => x).Distinct();

            if (GroupIdSymbol != null)
            {
                return Temp.Concat([GroupIdSymbol]);
            }
            else
            {
                return Temp;
            }
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion

        #region Internal Classes

        public class Aggregation(FunctionCall call, Signature signature, Symbol mask)
        {
            #region Public Properties

            public FunctionCall Call { get; } = call;

            public Signature Signature { get; } = signature;

            public Symbol Mask { get; } = mask;

            #endregion
            #region Constructors

            #endregion
        }
        #endregion
    }
}
