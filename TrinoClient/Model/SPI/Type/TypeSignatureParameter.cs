using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.TypeSignatureParameter.java
    /// </summary>
    public class TypeSignatureParameter
    {
        #region Public Properties

        public ParameterKind Kind { get; }

        public dynamic Value { get; }

        #endregion

        #region Constructors

        public TypeSignatureParameter(TypeSignature typeSignature)
        {
            Kind = ParameterKind.TYPE;
            Value = typeSignature ?? throw new ArgumentNullException(nameof(typeSignature));
        }

        public TypeSignatureParameter(long longLiteral)
        {
            Kind = ParameterKind.LONG;
            Value = longLiteral;
        }

        public TypeSignatureParameter(NamedTypeSignature namedTypeSignature)
        {
            Kind = ParameterKind.NAMED_TYPE;
            Value = namedTypeSignature ?? throw new ArgumentNullException(nameof(namedTypeSignature));
        }

        public TypeSignatureParameter(string variable)
        {
            Kind = ParameterKind.VARIABLE;
            Value = variable;
        }

        [JsonConstructor]
        private TypeSignatureParameter(ParameterKind kind, object value)
        {
            Kind = kind;
            Value = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null"); ;
        }

        #endregion

        #region Public Methods

        public bool IsTypeSignature()
        {
            return Kind == ParameterKind.TYPE;
        }

        public bool IsLongLiteral()
        {
            return Kind == ParameterKind.LONG;
        }

        public bool IsNamedTypeSignature()
        {
            return Kind == ParameterKind.NAMED_TYPE;
        }

        public bool IsVariable()
        {
            return Kind == ParameterKind.VARIABLE;
        }

        public TypeSignature GetTypeSignature()
        {
            return (TypeSignature)GetValue(ParameterKind.TYPE, typeof(TypeSignature));
        }

        public long GetLongLiteral()
        {
            return (long)GetValue(ParameterKind.LONG, typeof(long));
        }

        public NamedTypeSignature GetNamedTypeSignature()
        {
            return (NamedTypeSignature)GetValue(ParameterKind.NAMED_TYPE, typeof(NamedTypeSignature));
        }

        public string GetVariable()
        {
            return (string)GetValue(ParameterKind.VARIABLE, typeof(string));
        }

        public bool IsCalculated()
        {
            return Kind switch
            {
                ParameterKind.TYPE => GetTypeSignature().Calculated,
                ParameterKind.NAMED_TYPE => GetNamedTypeSignature().TypeSignature.Calculated,
                ParameterKind.LONG => false,
                ParameterKind.VARIABLE => true,
                _ => throw new ArgumentException($"Unexpected parameter kind: {Kind}"),
            };
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region Private Methods

        private object GetValue(ParameterKind expectedParameterKind, System.Type target)
        {
            return Convert.ChangeType(Value, target);
        }

        #endregion
    }
}
