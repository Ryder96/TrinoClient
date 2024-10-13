using Newtonsoft.Json;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.OperatorInfo.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IOperatorInfo
    {
        bool IsFinal();
    }
}
