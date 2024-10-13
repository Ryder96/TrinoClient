using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model
{
    /// <summary>
    /// From io.airlift.units.Duration.java
    /// </summary>
    [JsonConverter(typeof(TimeSpanConverter))]
    public class Duration
    {

    }
}
