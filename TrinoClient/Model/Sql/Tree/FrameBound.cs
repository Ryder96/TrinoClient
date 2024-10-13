using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.FrameBound.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.FrameBound.java
                                 /// </summary>
    public class FrameBound(NodeLocation location, FrameBoundType type, dynamic value, dynamic originalValue) : Node(location)
    {
        #region Public Properties

        public FrameBoundType Type { get; } = type;

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Value { get; } = value;

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic OriginalValue { get; } = originalValue;

        #endregion

        #region Constructors

        public FrameBound(FrameBoundType type) : this(null, type)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type) : this(location, type, null, null)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type, dynamic value) : this(location, type, (object)value, (object)value)
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
            FrameBound Other = (FrameBound)obj;

            return Object.Equals(Type, Other.Type) &&
                    object.Equals(Value, Other.Value);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Type, Value);
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", Type)
                .Add("value", Value)
                .ToString();
        }

        #endregion
    }
}
