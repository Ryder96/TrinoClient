using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.faceboook.presto.sql.planner.plan.TopNRowNumberNode.java
    /// </summary>
    public class TopNRowNumberNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Specification Specification { get; }

        public Symbol RowNumberSymbol { get; }

        public int MaxRowCountPerPartition { get; }

        public bool Partial { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TopNRowNumberNode(PlanNodeId id, PlanNode source, Specification specification, Symbol rowNumberSymbol, int maxRowCountPerPartition, bool partial, Symbol hashSymbol) : base(id)
        {
            ParameterCheck.OutOfRange(maxRowCountPerPartition > 0, "maxrowCountPerPartition", "Max row count per partition must be greater than 0.");

            Source = source ?? throw new ArgumentNullException(nameof(source));
            Specification = specification ?? throw new ArgumentNullException(nameof(specification));

            ParameterCheck.NonNull<OrderingScheme>(Specification.OrderingScheme, "specification", "The specification ordering scheme is absent.");

            RowNumberSymbol = rowNumberSymbol;
            MaxRowCountPerPartition = maxRowCountPerPartition;
            Partial = partial;
            HashSymbol = hashSymbol;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            if (!Partial)
            {
                return Source.GetOutputSymbols().Concat([RowNumberSymbol]);
            }
            else
            {
                return Source.GetOutputSymbols();
            }
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
