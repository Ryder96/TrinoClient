using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Symbol.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Symbol : IComparable<Symbol>
    {
        #region Public Properties

        public string Name { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Symbol(string name)
        {
            ParameterCheck.NotNullOrEmpty(name, "name");

            Name = name;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Symbol other)
        {
            return Name.CompareTo(other.Name);
        }

        #endregion
    }
}
