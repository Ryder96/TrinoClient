using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.MarkDistinctOperator.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.MarkDistinctOperator.java
                                 /// </summary>
    public class MarkDistinctNode(PlanNodeId id, PlanNode source, Symbol markerSymbol, IEnumerable<Symbol> distinctSymbols, Symbol hashSymbol) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source;

        public Symbol MarkerSymbol { get; } = markerSymbol;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; } = hashSymbol;

        public IEnumerable<Symbol> DistinctSymbols { get; } = distinctSymbols ?? throw new ArgumentNullException(nameof(distinctSymbols));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols().Concat([MarkerSymbol]);
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
