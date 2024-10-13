using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExplainAnalyzeNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.ExplainAnalyzeNode.java
                                 /// </summary>
    public class ExplainAnalyzeNode(PlanNodeId id, PlanNode source, Symbol outputSymbol, bool verbose) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public Symbol OutputSymbol { get; } = outputSymbol ?? throw new ArgumentNullException(nameof(outputSymbol));

        public bool Verbose { get; } = verbose;

        #endregion
        #region Constructors

        #endregion

        #region Public Properties

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            yield return OutputSymbol;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
