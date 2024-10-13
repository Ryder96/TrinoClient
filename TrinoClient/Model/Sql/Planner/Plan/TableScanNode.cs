using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.Metadata;
using TrinoClient.Model.SPI.Predicate;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableScanNode.java
    /// </summary>
    public class TableScanNode : PlanNode
    {
        #region Public Properties

        public TableHandle Table { get; }

        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Key is Symbol and Value is IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> Assignments { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TableLayoutHandle Layout { get; }

        /// <summary>
        /// TODO: TupleDomain<IColumnHandle>
        /// </summary>
        public TupleDomainPlaceHolder<dynamic> CurrentConstraint { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic OriginalConstraint { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableScanNode(
            PlanNodeId id,
            TableHandle table,
            IEnumerable<Symbol> outputSymbols,
            IDictionary<string, dynamic> assignments,
            TableLayoutHandle layout,
            TupleDomainPlaceHolder<dynamic> currentConstraint,
            dynamic originalConstraint
            ) : base(id)
        {
            Table = table ?? throw new ArgumentNullException(nameof(table));
            OutputSymbols = outputSymbols ?? throw new ArgumentNullException(nameof(outputSymbols));
            Assignments = assignments ?? throw new ArgumentNullException(nameof(assignments));
            OriginalConstraint = originalConstraint;
            Layout = layout ?? throw new ArgumentNullException(nameof(layout));
            CurrentConstraint = currentConstraint ?? throw new ArgumentNullException(nameof(currentConstraint));

            ParameterCheck.Check(OutputSymbols.All(x => Assignments.Keys.Contains(x.ToString())), "Assignments does not cover all of outputs.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return [];
        }

        #endregion
    }
}
