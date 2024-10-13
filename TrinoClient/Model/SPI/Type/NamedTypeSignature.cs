using Newtonsoft.Json;

namespace TrinoClient.Model.SPI.Type
{
    /// <summary>
    /// from com.facebook.presto.spi.type.NamedTypeSignature.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// from com.facebook.presto.spi.type.NamedTypeSignature.java
                                 /// </summary>
    public class NamedTypeSignature(string name, TypeSignature typeSignature)
    {
        #region Public Properties

        public string Name { get; } = name;

        public TypeSignature TypeSignature { get; } = typeSignature;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Name} {TypeSignature}";
        }

        #endregion
    }
}
