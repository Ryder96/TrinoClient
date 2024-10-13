using Newtonsoft.Json;
using System;
using TrinoClient.Model.Connector;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TableHandle.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.metadata.TableHandle.java
                                 /// </summary>
    public class TableHandle(ConnectorId connectorId, dynamic connectorHandle)
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; } = connectorId ?? throw new ArgumentNullException(nameof(connectorId));

        /// <summary>
        /// TODO: Supposed to be an IConnectorTableHandle
        /// </summary>
        public dynamic ConnectorHandle { get; } = connectorHandle ?? throw new ArgumentNullException(nameof(connectorHandle));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{ConnectorId}:{ConnectorHandle}";
        }

        #endregion
    }
}
