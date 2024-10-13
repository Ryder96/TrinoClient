using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SemiJoinNode.java
    /// </summary>
    public class SemiJoinNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public PlanNode FilteringSource { get; }

        public Symbol SourceJoinSymbol { get; }

        public Symbol FilteringSourceJoinSymbol { get; }

        public Symbol SemiJoinOutput { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol SourceHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol FilteringSourceHashSymbol { get; }

        public DistributionType DistributionType { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SemiJoinNode(
            PlanNodeId id,
            PlanNode source,
            PlanNode filteringSource,
            Symbol sourceJoinSymbol,
            Symbol filteringSourceJoinSymbol,
            Symbol semiJoinOutput,
            Symbol sourceHashSymbol,
            Symbol filteringSourceHashSymbol,
            DistributionType distributionType
            ) : base(id)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            FilteringSource = filteringSource ?? throw new ArgumentNullException(nameof(filteringSource));
            SourceJoinSymbol = sourceJoinSymbol ?? throw new ArgumentNullException(nameof(sourceJoinSymbol));
            FilteringSourceJoinSymbol = filteringSourceJoinSymbol ?? throw new ArgumentNullException(nameof(filteringSourceJoinSymbol));
            SemiJoinOutput = semiJoinOutput ?? throw new ArgumentNullException(nameof(semiJoinOutput));
            SourceHashSymbol = sourceHashSymbol;
            FilteringSourceHashSymbol = filteringSourceHashSymbol;
            DistributionType = distributionType;

            ParameterCheck.Check(Source.GetOutputSymbols().Contains(SourceJoinSymbol), "Source does not contain join symbol.");
            ParameterCheck.Check(FilteringSource.GetOutputSymbols().Contains(FilteringSourceJoinSymbol), "Filtering source does not contain filtering join symbol.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Source.GetOutputSymbols().Concat([SemiJoinOutput]);
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;

            yield return FilteringSource;
        }

        #endregion
    }
}
