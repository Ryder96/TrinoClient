using Newtonsoft.Json;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.LongVariableConstraint
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.metadata.LongVariableConstraint
                                 /// </summary>
    public class LongVariableConstraint(string name, string expression)
    {
        #region Public Properties

        public string Name { get; } = name;

        public string Expression { get; } = expression;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}:{Expression}";
        }

        #endregion
    }
}
