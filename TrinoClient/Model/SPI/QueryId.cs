using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.QueryId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class QueryId
    {
        #region Private Fields

        private static readonly Regex ID_PATTERN = new("[_a-z0-9]+");

        #endregion

        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryId(string id)
        {
            if (ValidateId(id))
            {
                Id = id;
            }
            else
            {
                throw new ArgumentException("The provided id is invalid.", nameof(id));
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Id;
        }

        public static bool ValidateId(string id)
        {
            ParameterCheck.NotNullOrEmpty(id, "id");

            return ID_PATTERN.IsMatch(id);
        }

        #endregion
    }
}
