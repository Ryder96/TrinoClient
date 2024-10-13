using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Connector;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Input.java
    /// </summary>
    public class Input
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; }

        public string Schema { get; }

        public string Table { get; }

        /// <summary>
        /// Not included in JSON serialization if not present
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public PrestoQueryConnectorInfo ConnectorInfo { get; set; }

        public IEnumerable<Column> Columns { get; }

        #endregion

        #region Constructors

        public Input(ConnectorId connectorId, string schema, string table, IEnumerable<Column> columns, PrestoQueryConnectorInfo connectorInfo = null)
        {
            if (string.IsNullOrEmpty(schema))
            {
                throw new ArgumentNullException(nameof(schema), "The schema cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException(nameof(table), "The table cannot be null or empty.");
            }

            ConnectorId = connectorId ?? throw new ArgumentNullException(nameof(connectorId), "The connector id cannot be null.");
            Columns = columns ?? throw new ArgumentNullException(nameof(columns), "The columns cannot be null.");
            ConnectorInfo = connectorInfo;
        }

        #endregion

        #region Child Classes

        public class PrestoQueryConnectorInfo
        {
            public IEnumerable<string> PartitionIds { get; set; }
        }

        #endregion
    }
}
