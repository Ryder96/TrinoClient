using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.DistinctLimitNode.java
    /// </summary>
    public class DistinctLimitNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public long Limit { get; }

        public bool Partial { get; }

        public IEnumerable<Symbol> DistinctSymbols { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DistinctLimitNode(PlanNodeId id, PlanNode source, long limit, bool partial, IEnumerable<Symbol> distinctSymbols, Symbol hashSymbol) : base(id)
        {
            ParameterCheck.OutOfRange(limit >= 0, "The limit cannot be less than zero.");

            Source = source ?? throw new ArgumentNullException(nameof(source));
            Limit = limit;
            Partial = partial;
            DistinctSymbols = distinctSymbols;
            HashSymbol = hashSymbol;

            if (HashSymbol != null && DistinctSymbols.Contains(HashSymbol))
            {
                throw new ArgumentException("Distinct symbols should not contain hash symbol.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            if (HashSymbol != null)
            {
                return DistinctSymbols.Concat([HashSymbol]);
            }
            else
            {
                return DistinctSymbols;
            }
        }

        #endregion
    }
}
