using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IntersectNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.IntersectNode.java
                                 /// </summary>
    public class IntersectNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : SetOperationNode(id, sources, outputToInputs, outputs)
    {

        #region Constructors

        #endregion
    }
}
