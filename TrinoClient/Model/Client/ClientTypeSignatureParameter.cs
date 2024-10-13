using Newtonsoft.Json;
using System;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ClientTypeSignatureParameter.java
    /// </summary>
    public class ClientTypeSignatureParameter
    {
        #region Public Properties

        public ParameterKind Kind { get; }

        public object Value { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ClientTypeSignatureParameter(ParameterKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        public ClientTypeSignatureParameter(TypeSignatureParameter typeParameterSignature)
        {
            Kind = typeParameterSignature.Kind;

            switch (Kind)
            {
                case ParameterKind.TYPE:
                    {
                        Value = new ClientTypeSignature(typeParameterSignature.GetTypeSignature());
                        break;
                    }
                case ParameterKind.LONG:
                    {
                        Value = typeParameterSignature.GetLongLiteral();
                        break;
                    }
                case ParameterKind.NAMED_TYPE:
                    {
                        Value = typeParameterSignature.GetNamedTypeSignature();
                        break;
                    }
                case ParameterKind.VARIABLE:
                    {
                        Value = typeParameterSignature.GetVariable();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException($"Unknown type signature kind {Kind}.");
                    }
            }
        }

        #endregion

        #region Public Methods

        public ClientTypeSignature GetTypeSignature()
        {
            return (ClientTypeSignature)GetValue(ParameterKind.TYPE, typeof(ClientTypeSignature));
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
