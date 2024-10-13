using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SetOperationNode.java
    /// </summary>
    public class SetOperationNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : PlanNode(id)
    {
        #region Public Properties

        public IEnumerable<PlanNode> Sources { get; } = sources ?? throw new ArgumentNullException(nameof(sources));

        public IEnumerable<KeyValuePair<Symbol, Symbol>> OutputToInputs { get; } = outputToInputs ?? throw new ArgumentNullException(nameof(outputToInputs));

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
            return Sources;
        }

        #endregion
    }
}
