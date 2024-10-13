using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Metadata;
using TrinoClient.Model.SPI.Predicate;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IndexSourceNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.IndexSourceNode.java
                                 /// </summary>
    public class IndexSourceNode(
        PlanNodeId id,
        IndexHandle indexHandle,
        TableHandle tableHandle,
        TableLayoutHandle tableLayout,
        HashSet<Symbol> lookupSymbols,
        IEnumerable<Symbol> outputSymbols,
        IDictionary<string, dynamic> assignments,
        TupleDomainPlaceHolder<dynamic> currentConstraint
            ) : PlanNode(id)
    {
        #region Public Properties

        public IndexHandle IndexHandle { get; } = indexHandle ?? throw new ArgumentNullException(nameof(indexHandle));

        public TableHandle TableHandle { get; } = tableHandle ?? throw new ArgumentNullException("tableHanlde");

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TableLayoutHandle TableLayout { get; } = tableLayout;

        public HashSet<Symbol> LookupSymbols { get; } = lookupSymbols ?? throw new ArgumentNullException(nameof(lookupSymbols));

        public IEnumerable<Symbol> OutputSymbols { get; } = outputSymbols ?? throw new ArgumentNullException(nameof(outputSymbols));

        /// <summary>
        /// TODO: Key is supposed to be Symbol, Key is IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> Assignments { get; } = assignments ?? throw new ArgumentNullException(nameof(assignments));

        /// <summary>
        /// TODO: TupleDomain<IColumnHandle>
        /// </summary>
        public TupleDomainPlaceHolder<dynamic> CurrentConstraint { get; } = currentConstraint ?? throw new ArgumentNullException(nameof(currentConstraint));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return [];
        }

        #endregion
    }
}
