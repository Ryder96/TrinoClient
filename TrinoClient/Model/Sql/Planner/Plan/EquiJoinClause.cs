using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.JoinNode.java (internal class EquiJoinClause)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.JoinNode.java (internal class EquiJoinClause)
                                 /// </summary>
    public class EquiJoinClause(Symbol left, Symbol right)
    {
        #region Public Properties

        public Symbol Left { get; } = left ?? throw new ArgumentNullException(nameof(left));

        public Symbol Right { get; } = right ?? throw new ArgumentNullException(nameof(right));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Left.ToString()} = {Right.ToString()}";
        }

        #endregion
    }
}
