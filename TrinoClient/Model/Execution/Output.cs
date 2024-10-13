using Newtonsoft.Json;
using System;
using TrinoClient.Model.Connector;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Output.java
    /// </summary>
    public class Output
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; }

        public string Schema { get; }

        public string Table { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Output(ConnectorId connectorId, string schema, string table)
        {
            if (string.IsNullOrEmpty(schema))
            {
                throw new ArgumentNullException(nameof(schema), "Schema cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException(nameof(table), "Table cannot be null or empty.");
            }

            ConnectorId = connectorId ?? throw new ArgumentNullException(nameof(connectorId), "The connector id cannot be null.");
        }

        #endregion
    }
}
