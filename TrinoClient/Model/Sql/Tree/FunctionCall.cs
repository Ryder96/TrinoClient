using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.FunctionCall.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.FunctionCall.java
                                 /// </summary>
    public class FunctionCall(NodeLocation location, QualifiedName name, Window window, object filter, OrderBy orderBy, bool distinct, IEnumerable<object> arguments) : Expression(location)
    {
        #region Public Properties

        public QualifiedName Name { get; } = name;

        public Window Window { get; } = window;

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Filter { get; } = filter;

        public OrderBy OrderBy { get; } = orderBy;

        public bool Distinct { get; } = distinct;

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public IEnumerable<dynamic> Arguments { get; } = arguments;

        #endregion

        #region Constructors

        public FunctionCall(QualifiedName name, IEnumerable<object> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(NodeLocation location, QualifiedName name, IEnumerable<object> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<object> arguments)
            : this(null, name, null, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<object> arguments, object filter)
            : this(null, name, null, filter, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, bool distinct, IEnumerable<object> arguments)
            : this(null, name, window, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, object filter, OrderBy orderBy, bool distinct, IEnumerable<object> arguments)
            : this(null, name, window, filter, orderBy, distinct, arguments)
        { }

        #endregion

        #region Public Properties

        public override IEnumerable<Node> GetChildren()
        {
            if (Window != null)
            {
                yield return Window;
            }

            if (Filter != null)
            {
                yield return Filter;
            }

            foreach (SortItem Item in OrderBy.SortItems)
            {
                yield return Item;
            }

            foreach (Expression Item in Arguments)
            {
                yield return Item;
            }
        }

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

            FunctionCall Other = (FunctionCall)obj;

            return object.Equals(Name, Other.Name) &&
                    object.Equals(Window, Other.Window) &&
                    object.Equals(Filter, Other.Filter) &&
                    object.Equals(OrderBy, Other.OrderBy) &&
                    object.Equals(Distinct, Other.Distinct) &&
                    object.Equals(Arguments, Other.Arguments);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Name, Distinct, Window, Filter, OrderBy, Arguments);
        }

        #endregion
    }
}
