using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableWriterNode.java
    /// </summary>
    public class TableWriterNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public WriterTarget Target { get; }

        public IEnumerable<Symbol> Outputs { get; }

        public IEnumerable<Symbol> Columns { get; }

        public IEnumerable<string> ColumnNames { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public PartitioningScheme PartitioningScheme { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableWriterNode(PlanNodeId id, PlanNode source, WriterTarget target, IEnumerable<Symbol> outputs, IEnumerable<Symbol> columns, IEnumerable<string> columnNames, PartitioningScheme partitioningScheme) : base(id)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            ColumnNames = columnNames ?? throw new ArgumentNullException(nameof(columnNames));

            if (Columns.Count() != ColumnNames.Count())
            {
                throw new ArgumentException("Columns and column names sizes don't match.");
            }

            Outputs = outputs ?? throw new ArgumentNullException(nameof(outputs));
            PartitioningScheme = partitioningScheme;
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
