using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IndexJoinNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.IndexJoinNode.java
                                 /// </summary>
    public class IndexJoinNode(
        PlanNodeId id,
        IndexJoinNodeType type,
        PlanNode probeSource,
        PlanNode indexSource,
        IEnumerable<IndexJoinNode.EquiJoinClause> criteria,
        Symbol probeHashSymbol,
        Symbol indexHashSymbol
            ) : PlanNode(id)
    {
        #region Public Properties

        public IndexJoinNodeType Type { get; } = type;

        public PlanNode ProbeSource { get; } = probeSource ?? throw new ArgumentNullException(nameof(probeSource));

        public PlanNode IndexSource { get; } = indexSource ?? throw new ArgumentNullException(nameof(indexSource));

        public IEnumerable<EquiJoinClause> Criteria { get; } = criteria ?? throw new ArgumentNullException(nameof(criteria));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol ProbeHashSymbol { get; } = probeHashSymbol;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol IndexHashSymbol { get; } = indexHashSymbol;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return ProbeSource.GetOutputSymbols().Concat(IndexSource.GetOutputSymbols());
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return ProbeSource;
            yield return IndexSource;
        }

        #endregion

        #region Internal Classes

        public class EquiJoinClause(Symbol probe, Symbol index)
        {
            #region Public Properties

            public Symbol Probe { get; } = probe ?? throw new ArgumentNullException(nameof(probe));

            public Symbol Index { get; } = index ?? throw new ArgumentNullException(nameof(index));

            #endregion
            #region Constructors

            #endregion
        }

        #endregion
    }
}
