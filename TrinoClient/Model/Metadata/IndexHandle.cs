using Newtonsoft.Json;
using System;
using TrinoClient.Model.Connector;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.IndexHandle.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.metadata.IndexHandle.java
                                 /// </summary>
    public sealed class IndexHandle(ConnectorId connectorId, dynamic transactionHandle, dynamic connectorHandle)
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; } = connectorId ?? throw new ArgumentNullException(nameof(connectorId));

        /// <summary>
        /// TODO: Supposed to be IConnectorTransactionHandle
        /// </summary>
        public dynamic TransactionHandle { get; } = transactionHandle ?? throw new ArgumentNullException(nameof(transactionHandle));

        /// <summary>
        /// TODO: Supposed to be IConnectorIndexHandle
        /// </summary>
        public dynamic ConnectorHandle { get; } = connectorHandle ?? throw new ArgumentNullException(nameof(connectorHandle));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{ConnectorId}:{TransactionHandle}:{ConnectorHandle}";
        }

        #endregion
    }
}
