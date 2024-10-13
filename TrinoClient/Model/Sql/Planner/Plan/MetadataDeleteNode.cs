using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Metadata;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.MetadataDeleteNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.MetadataDeleteNode.java
                                 /// </summary>
    public class MetadataDeleteNode(PlanNodeId id, DeleteHandle target, Symbol output, TableLayoutHandle tableLayout) : PlanNode(id)
    {
        #region Public Properties

        public DeleteHandle Target { get; } = target ?? throw new ArgumentNullException(nameof(target));

        public Symbol Output { get; } = output ?? throw new ArgumentNullException(nameof(output));

        public TableLayoutHandle TableLayout { get; } = tableLayout ?? throw new ArgumentNullException(nameof(tableLayout));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            yield return Output;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return [];
        }

        #endregion
    }
}
