using Newtonsoft.Json;
using System.Text;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TypeVariableConstraint.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.metadata.TypeVariableConstraint.java
                                 /// </summary>
    public class TypeVariableConstraint(string name, bool comparableRequired, bool orderableRequired, string variadicBound)
    {
        #region Public Properties

        public string Name { get; } = name;

        public bool ComparableRequired { get; } = comparableRequired;

        public bool OrderableRequired { get; } = orderableRequired;

        public string VariadicBound { get; } = variadicBound;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            StringBuilder SB = new(Name);

            if (ComparableRequired)
            {
                SB.Append(":comparable");
            }

            if (OrderableRequired)
            {
                SB.Append(":orderable");
            }

            if (!string.IsNullOrEmpty(VariadicBound))
            {
                SB.Append($":{VariadicBound}<*>");
            }

            return SB.ToString();
        }

        #endregion
    }
}
