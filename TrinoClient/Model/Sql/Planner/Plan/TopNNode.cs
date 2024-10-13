using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TopNNode.java
    /// </summary>
    public class TopNNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public long Count { get; }

        public OrderingScheme OrderingScheme { get; }

        public Step Step { get; set; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TopNNode(PlanNodeId id, PlanNode source, long count, OrderingScheme orderingScheme, Step step) : base(id)
        {
            ParameterCheck.OutOfRange(count >= 0, "count", "Count must be positive.");
            ParameterCheck.OutOfRange(count <= int.MaxValue, "count", $"ORDER BY LIMIT > {int.MaxValue} is not supported.");

            Source = source ?? throw new ArgumentNullException(nameof(source));
            Count = count;
            OrderingScheme = orderingScheme ?? throw new ArgumentNullException(nameof(orderingScheme));
            Step = step;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
