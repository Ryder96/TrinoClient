using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.RowNumberNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.RowNumberNode.java
                                 /// </summary>
    public class RowNumberNode(PlanNodeId id, PlanNode source, IEnumerable<Symbol> partitionBy, Symbol rowNumberSymbol, int maxRowCountPerPartition, Symbol hashSymbol) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public IEnumerable<Symbol> PartitionBy { get; } = partitionBy ?? throw new ArgumentNullException(nameof(partitionBy));

        public Symbol RowNumberSymbol { get; } = rowNumberSymbol ?? throw new ArgumentNullException(nameof(rowNumberSymbol));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public int MaxRowCountPerPartition { get; } = maxRowCountPerPartition;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; } = hashSymbol; // ?? throw new ArgumentNullException("hashSymbol");

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols().Concat([RowNumberSymbol]);
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
