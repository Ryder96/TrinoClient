using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.SortItem.java
    /// </summary>
    public class SortItem(NodeLocation location, object sortKey, Ordering ordering, NullOrdering nullOrdering) : Node(location)
    {
        #region Public Properties

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic SortKey { get; } = sortKey;

        public Ordering Ordering { get; } = ordering;

        public NullOrdering NullOrdering { get; } = nullOrdering;

        #endregion

        #region Constructors

        public SortItem(object sortKey, Ordering ordering, NullOrdering nullOrdering) : this(null, sortKey, ordering, nullOrdering)
        { }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SortItem Other = (SortItem)obj;

            return object.Equals(SortKey, Other.SortKey) &&
                    (Ordering == Other.Ordering) &&
                    (NullOrdering == Other.NullOrdering);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(SortKey, Ordering, NullOrdering);
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("sortKey", SortKey)
                .Add("ordering", Ordering)
                .Add("nullOrdering", NullOrdering)
                .ToString();
        }

        public override IEnumerable<Node> GetChildren()
        {
            return [SortKey];
        }

        #endregion
    }
}
