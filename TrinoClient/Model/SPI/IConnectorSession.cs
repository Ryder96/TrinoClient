using Newtonsoft.Json;
using System.Globalization;
using TrinoClient.Model.SPI.Security;
using TrinoClient.Model.SPI.Type;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorSession.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorSession
    {
        string GetQueryId();

        string GetSource();

        string GetUser();

        Identity GetIdentity();

        TimeZoneKey GetTimeZoneKey();

        CultureInfo GetLocale();

        long GetStartTime();

        object GetProperty(string name, System.Type type);
    }
}
