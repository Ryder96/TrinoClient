using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.AssignUniqueId.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.AssignUniqueId.java
                                 /// </summary>
    public class AssignUniqueId(PlanNodeId id, PlanNode source, Symbol idColumn) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public Symbol IdColumn { get; } = idColumn ?? throw new ArgumentNullException(nameof(idColumn));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols().Concat([IdColumn]);
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
