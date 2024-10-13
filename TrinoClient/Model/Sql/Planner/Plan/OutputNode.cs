using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.OutputNode.java
    /// </summary>
    public class OutputNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<string> Columns { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public OutputNode(PlanNodeId id, PlanNode source, IEnumerable<string> columns, IEnumerable<Symbol> outputs) : base(id)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            Outputs = outputs ?? throw new ArgumentNullException(nameof(outputs));

            if (Columns.Count() != Outputs.Count())
            {
                throw new ArgumentException("Column names and assignments sizes don't match.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return Source;
        }

        #endregion
    }
}
