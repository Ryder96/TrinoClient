using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.Column.java
    /// </summary>
    public class Column
    {
        #region Public Properties

        public string Name { get; }

        public string Type { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ClientTypeSignature TypeSignature { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Column(string name, string type, ClientTypeSignature typeSignature)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Name = name;
            Type = type;
            TypeSignature = typeSignature;
        }

        #endregion
    }
}
