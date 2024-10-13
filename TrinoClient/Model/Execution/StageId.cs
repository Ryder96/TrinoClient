using Newtonsoft.Json;
using System;
using TrinoClient.Model.SPI;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class StageId
    {
        #region Public Properties

        public QueryId QueryId { get; }

        public int Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StageId(string combinedId)
        {
            string[] Parts = combinedId.Split('.');

            if (Parts.Length < 2)
            {
                throw new ArgumentException("The combined id must be a query id and id separated with a '.' character.");
            }

            QueryId = new QueryId(Parts[0]);
            Id = int.Parse(Parts[1]);
        }

        public StageId(string queryId, int id)
        {
            QueryId = new QueryId(queryId);
            Id = id;
        }

        public StageId(QueryId queryId, int id)
        {
            QueryId = queryId ?? throw new ArgumentNullException(nameof(queryId), "Query id cannot be null.");
            Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{QueryId.ToString()}.{Id}";
        }

        #endregion
    }
}
