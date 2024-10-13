using Newtonsoft.Json;
using TrinoClient.Model.Sql.Tree;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Frame)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Frame)
                                 /// </summary>
    public class Frame(WindowFrameType type, FrameBoundType startType, Symbol startValue, FrameBoundType endType, Symbol endValue)
    {
        #region Public Properties

        public WindowFrameType Type { get; } = type;

        public FrameBoundType StartType { get; } = startType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol StartValue { get; } = startValue;

        public FrameBoundType EndType { get; } = endType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol EndValue { get; } = endValue;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return Hashing.Hash(
                Type,
                StartType,
                EndType,
                StartValue,
                EndValue);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Frame) != obj.GetType())
            {
                return false;
            }

            Frame Other = (Frame)obj;

            return object.Equals(Type, Other.Type) &&
                    object.Equals(StartType, Other.StartType) &&
                    object.Equals(StartValue, Other.StartValue) &&
                    object.Equals(EndType, Other.EndType) &&
                    object.Equals(EndValue, Other.EndValue);
        }

        #endregion
    }
}
