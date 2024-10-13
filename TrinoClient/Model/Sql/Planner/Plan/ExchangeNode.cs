using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExchangeNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.ExchangeNode.java
                                 /// </summary>
    public class ExchangeNode(PlanNodeId id, ExchangeType type, ExchangeScope scope, PartitioningScheme partitioningScheme, IEnumerable<PlanNode> sources, IEnumerable<List<Symbol>> inputs) : PlanNode(id)
    {
        #region Public Properties

        public ExchangeType Type { get; } = type;

        public ExchangeScope Scope { get; } = scope;

        public IEnumerable<PlanNode> Sources { get; } = sources ?? throw new ArgumentNullException(nameof(sources));

        public PartitioningScheme PartitioningScheme { get; } = partitioningScheme ?? throw new ArgumentNullException(nameof(partitioningScheme));

        public IEnumerable<List<Symbol>> Inputs { get; } = inputs ?? throw new ArgumentNullException(nameof(inputs));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            return Sources;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return PartitioningScheme.OutputLayout;
        }

        #endregion
    }
}
