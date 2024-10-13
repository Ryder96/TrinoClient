using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java
    /// </summary>
    public class WindowNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Specification Specification { get; }

        /// <summary>
        /// TODO: Key should be Symbol
        /// </summary>
        public IDictionary<string, Function> WindowFunctions { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        public HashSet<Symbol> PrePartitionedInputs { get; }

        public int PreSortedOrderPrefix { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public WindowNode(
            PlanNodeId id,
            PlanNode source,
            Specification specification,
            IDictionary<string, Function> windowFunctions,
            Symbol hashSymbol,
            HashSet<Symbol> prePartitionedInputs,
            int preSortedOrderPrefix
            ) : base(id)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            PrePartitionedInputs = prePartitionedInputs;
            Specification = specification ?? throw new ArgumentNullException(nameof(specification));
            PreSortedOrderPrefix = preSortedOrderPrefix;

            ParameterCheck.Check(PrePartitionedInputs.All(x => Specification.PartitionBy.Contains(x)), "Prepartitioned inputs must be contained in partitionBy.");


            ParameterCheck.Check(preSortedOrderPrefix == 0 ||
                (Specification.OrderingScheme != null && PreSortedOrderPrefix <= Specification.OrderingScheme.OrderBy.Count()), "Cannot have sorted more symbols than those requested.");
            ParameterCheck.Check(preSortedOrderPrefix == 0 || PrePartitionedInputs.Equals(Specification.PartitionBy),
                "Presorted order prefix can only be greater than zero if all partition symbols are pre-partitioned");

            WindowFunctions = windowFunctions ?? throw new ArgumentNullException(nameof(windowFunctions));
            HashSymbol = hashSymbol;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols().Concat(WindowFunctions.Keys.Select(x => new Symbol(x)));
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
