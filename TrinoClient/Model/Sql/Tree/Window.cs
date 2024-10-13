using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.Window.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.Window.java
                                 /// </summary>
    public class Window(NodeLocation location, IEnumerable<object> partitionBy, OrderBy orderBy, WindowFrame frame) : Node(location)
    {
        #region Public Properties

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public IEnumerable<dynamic> PartitionBy { get; } = partitionBy ?? throw new ArgumentNullException(nameof(partitionBy));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public OrderBy OrderBy { get; } = orderBy; // These are actually optional ?? throw new ArgumentNullException("orderBy");

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public WindowFrame Frame { get; } = frame; // These are actually optional ?? throw new ArgumentNullException("frame");

        #endregion

        #region Constructors

        public Window(IEnumerable<object> partitionBy, OrderBy orderBy, WindowFrame frame) : this(null, partitionBy, orderBy, frame)
        {
        }

        #endregion

        #region Public Properties

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

            Window Other = (Window)obj;

            return Object.Equals(PartitionBy, Other.PartitionBy) &&
                    Object.Equals(OrderBy, Other.OrderBy) &&
                    Object.Equals(Frame, Other.Frame);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(PartitionBy, OrderBy, Frame);
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("partitionBy", PartitionBy)
                .Add("orderBy", OrderBy)
                .Add("frame", Frame)
                .ToString();
        }

        public override IEnumerable<Node> GetChildren()
        {
            foreach (Node Item in PartitionBy)
            {
                yield return Item;
            }

            if (OrderBy != null)
            {
                yield return OrderBy;
            }

            if (Frame != null)
            {
                yield return Frame;
            }
        }

        #endregion
    }
}
