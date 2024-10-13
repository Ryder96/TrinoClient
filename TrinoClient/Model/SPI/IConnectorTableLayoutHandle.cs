using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorTableLayoutHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorTableLayoutHandle
    {
        // Intentionally empty
    }
}
