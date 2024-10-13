using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableFinishNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.TableFinishNode.java
                                 /// </summary>
    public class TableFinishNode(PlanNodeId id, PlanNode source, WriterTarget target, IEnumerable<Symbol> outputs) : PlanNode(id)
    {
        #region Public Properties

        public PlanNode Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public WriterTarget Target { get; } = target;

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
            yield return Source;
        }

        #endregion
    }
}
