using Newtonsoft.Json;
using System;
using TrinoClient.Model.Connector;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TableLayoutHandle.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.metadata.TableLayoutHandle.java
                                 /// </summary>
    public sealed class TableLayoutHandle(ConnectorId connectorId, dynamic transactionHandle, dynamic connectorHandle)
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; } = connectorId ?? throw new ArgumentNullException(nameof(connectorId));

        /// <summary>
        /// TODO: Supposed to be IConnectorTransactionHandle
        /// </summary>
        public dynamic TransactionHandle { get; } = transactionHandle ?? throw new ArgumentNullException(nameof(transactionHandle));

        /// <summary>
        /// TODO: Supposed to be IConnectorTableLayoutHandle
        /// </summary>
        public dynamic ConnectorHandle { get; } = connectorHandle ?? throw new ArgumentNullException(nameof(connectorHandle));

        #endregion
        #region Constructors

        #endregion
    }
}
