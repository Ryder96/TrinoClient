using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.OrderBy.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.OrderBy.java
                                 /// </summary>
    public class OrderBy(NodeLocation location, IEnumerable<SortItem> sortItems) : Node(location)
    {
        #region Public Properties

        public IEnumerable<SortItem> SortItems { get; } = sortItems ?? throw new ArgumentNullException(nameof(sortItems));

        #endregion

        #region Constructors

        public OrderBy(IEnumerable<SortItem> sortItems) : this(null, sortItems)
        { }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if ((obj == null) || (GetType() != obj.GetType()))
            {
                return false;
            }

            OrderBy Other = (OrderBy)obj;
            return Object.Equals(SortItems, Other.SortItems);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(SortItems);
        }

        public override IEnumerable<Node> GetChildren()
        {
            return SortItems;
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("sortItems", SortItems)
                .ToString();
        }

        #endregion
    }
}
