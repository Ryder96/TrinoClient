using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.UnnestNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.UnnestNode.java
                                 /// </summary>
    public class UnnestNode(PlanNodeId id, PlanNode source, IEnumerable<Symbol> replicateSymbols, IDictionary<string, List<Symbol>> unnestSymbols, Symbol ordinalitySymbol) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public IEnumerable<Symbol> ReplicateSymbols { get; } = replicateSymbols ?? throw new ArgumentNullException(nameof(replicateSymbols));

        /// <summary>
        /// TODO: Key should be Symbol
        /// </summary>
        public IDictionary<string, List<Symbol>> UnnestSymbols { get; } = unnestSymbols ?? throw new ArgumentNullException(nameof(unnestSymbols));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol OrdinalitySymbol { get; } = ordinalitySymbol;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            IEnumerable<Symbol> Temp = ReplicateSymbols.Concat(UnnestSymbols.Keys.Select(x => new Symbol(x)));

            if (OrdinalitySymbol != null)
            {
                Temp.Concat([OrdinalitySymbol]);
            }

            return Temp;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
