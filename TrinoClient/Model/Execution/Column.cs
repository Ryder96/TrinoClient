using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Column.java
    /// </summary>
    public class Column
    {
        #region Public Properties

        public string Name { get; }

        public string Type { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Column(string name, string type)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "The name cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type), "The type cannot be null or empty.");
            }

            Name = name;
            Type = type;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("name", Name)
                .Add("type", Type)
                .ToString();
        }

        #endregion
    }
}
