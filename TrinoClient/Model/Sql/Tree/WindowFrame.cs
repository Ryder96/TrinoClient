using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.WindowFrame.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.WindowFrame.java
                                 /// </summary>
    public class WindowFrame(NodeLocation location, WindowFrameType type, FrameBound start, FrameBound end) : Node(location)
    {
        #region Public Properties

        public WindowFrameType Type { get; } = type;

        public FrameBound Start { get; } = start ?? throw new ArgumentNullException(nameof(start));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FrameBound End { get; } = end; // this is optional ?? throw new ArgumentNullException("end")

        #endregion

        #region Constructors

        public WindowFrame(WindowFrameType type, FrameBound start, FrameBound end) : this(null, type, start, end)
        {
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Node> GetChildren()
        {
            yield return Start;

            if (End != null)
            {
                yield return End;
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

            WindowFrame Other = (WindowFrame)obj;

            return Object.Equals(Type, Other.Type) &&
                    Object.Equals(Start, Other.Start) &&
                    Object.Equals(End, Other.End);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Type, Start, End);
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", Type)
                .Add("start", Start)
                .Add("end", End)
                .ToString();
        }

        #endregion
    }
}
