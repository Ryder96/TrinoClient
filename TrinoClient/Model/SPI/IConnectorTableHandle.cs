using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorTableHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorTableHandle
    {
        // Intentionally empty
    }
}
