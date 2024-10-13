using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.DeleteNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.DeleteNode.java
                                 /// </summary>
    public class DeleteNode(PlanNodeId id, PlanNode source, DeleteHandle target, Symbol rowId, IEnumerable<Symbol> outputs) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public DeleteHandle Target { get; } = target ?? throw new ArgumentNullException(nameof(target));

        public Symbol RowId { get; } = rowId ?? throw new ArgumentNullException(nameof(rowId));

        public IEnumerable<Symbol> Outputs { get; } = outputs ?? throw new ArgumentNullException(nameof(outputs));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
