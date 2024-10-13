using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExceptNode.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.ExceptNode.java
                                 /// </summary>
    public class ExceptNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : SetOperationNode(id, sources, outputToInputs, outputs)
    {

        #region Constructors

        #endregion

        #region Public Methods

        #endregion
    }
}
