using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.Node.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.Node.java
                                 /// </summary>
    public abstract class Node(NodeLocation location)
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public NodeLocation Location { get; } = location;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        internal protected virtual R Accept<R, C>(AstVisitor<R, C> visitor, C context)
        {
            return visitor.VisitNode(this, context);
        }

        public abstract IEnumerable<Node> GetChildren();

        // Force subclasses to have a proper equals and hashcode implementation
        public abstract override int GetHashCode();

        public abstract override bool Equals(object obj);

        public abstract override string ToString();

        #endregion
    }
}
