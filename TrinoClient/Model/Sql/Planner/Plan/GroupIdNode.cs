using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.GroupIdNode.java
    /// </summary>
    public class GroupIdNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<List<Symbol>> GroupingSets { get; }

        /// <summary>
        /// TODO: Key is supposed to be Symbol
        /// </summary>
        public IDictionary<string, Symbol> GroupingSetMappings { get; }


        /// <summary>
        /// TODO: Key is supposed to be Symbol
        /// </summary>
        public IDictionary<string, Symbol> ArgumentMappings { get; }

        public Symbol GroupIdSymbol { get; }

        #endregion

        #region Constructors

        public GroupIdNode(
            PlanNodeId id,
            PlanNode source,
            IEnumerable<List<Symbol>> groupingSets,
            IDictionary<string, Symbol> groupingSetMappings,
            IDictionary<string, Symbol> argumentMappings,
            Symbol groupIdSymbol
            ) : base(id)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            GroupingSets = groupingSets ?? throw new ArgumentNullException(nameof(groupingSets));
            GroupingSetMappings = groupingSetMappings ?? throw new ArgumentNullException(nameof(groupingSetMappings));
            ArgumentMappings = argumentMappings ?? throw new ArgumentNullException(nameof(argumentMappings));
            GroupIdSymbol = groupIdSymbol ?? throw new ArgumentNullException(nameof(groupIdSymbol));

            if (GroupingSetMappings.Keys.Intersect(ArgumentMappings.Keys).Any())
            {
                throw new ArgumentException("The argument outputs and grouping outputs must be a disjoint set.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return GroupingSets.SelectMany(x => x).Concat(ArgumentMappings.Keys.Select(x => new Symbol(x))).Concat([GroupIdSymbol]);
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
