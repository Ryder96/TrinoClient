using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI.Connector
{
    /// <summary>
    /// From com.facebook.presto.spi.connector.ConnectorTransactionHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorTransactionHandle
    {
        //Intentionally empty
    }
}
