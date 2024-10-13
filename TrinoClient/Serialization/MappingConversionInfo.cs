using System;

namespace TrinoClient.Serialization
{
    public class MappingConversionInfo(Type type, MappingConversionInfo.PrestoTypeConverter converter)
    {
        public delegate object PrestoTypeConverter(string value);

        public PrestoTypeConverter Converter { get; set; } = converter ?? throw new ArgumentNullException("The converter cannot be null.");

        public Type DotNetType { get; set; } = type;
    }
}
