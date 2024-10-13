using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ClientTypeSignature.java
    /// </summary>
    public class ClientTypeSignature
    {
        #region Private Fields

        private static readonly Regex PATTERN = new(".*(?:[<>,].*)?");

        #endregion

        #region Public Properties

        public string RawType { get; }

        [Obsolete("This property is deprecated and clients should switch to Arguments}")]
        public IEnumerable<ClientTypeSignature> TypeArguments
        {
            get
            {
                List<ClientTypeSignature> Results = [];

                foreach (ClientTypeSignatureParameter Arg in Arguments)
                {
                    switch (Arg.Kind)
                    {
                        case ParameterKind.TYPE:
                            {
                                Results.Add(Arg.GetTypeSignature());
                                break;
                            }
                        case ParameterKind.NAMED_TYPE:
                            {
                                Results.Add(new ClientTypeSignature(Arg.GetNamedTypeSignature().TypeSignature));
                                break;
                            }
                        default:
                            {
                                return [];
                            }

                    }
                }

                return Results;
            }
        }

        [Obsolete("This property is deprecated and clients should switch to Arguments}")]
        public IEnumerable<object> LiteralArguments
        {
            get
            {
                List<object> Results = [];

                foreach (ClientTypeSignatureParameter Arg in Arguments)
                {
                    switch (Arg.Kind)
                    {
                        case ParameterKind.NAMED_TYPE:
                            {
                                Results.Add(Arg.GetNamedTypeSignature().Name);
                                break;
                            }
                        default:
                            {
                                return [];
                            }

                    }
                }

                return Results;
            }
        }

        public IEnumerable<ClientTypeSignatureParameter> Arguments { get; }

        #endregion

        #region Constructors

        public ClientTypeSignature(TypeSignature typeSignature) : this(typeSignature.Base, typeSignature.Parameters.Select(x => new ClientTypeSignatureParameter(x)))
        {
        }

        public ClientTypeSignature(string rawType, IEnumerable<ClientTypeSignatureParameter> arguments) : this(rawType, [], [], arguments)
        {
        }

        [JsonConstructor]
        public ClientTypeSignature(string rawType, IEnumerable<ClientTypeSignature> typeArguments, IEnumerable<object> literalArguments, IEnumerable<ClientTypeSignatureParameter> arguments)
        {
            if (string.IsNullOrEmpty(rawType))
            {
                throw new ArgumentNullException(nameof(rawType));
            }

            Match RegexMatch = PATTERN.Match(rawType);

            if (!RegexMatch.Success)
            {
                throw new FormatException($"Bad characters in rawType: {rawType}.");
            }

            RawType = rawType;

            if (arguments != null)
            {
                Arguments = arguments;
            }
            else
            {
                ArgumentNullException.ThrowIfNull(typeArguments);

                ArgumentNullException.ThrowIfNull(literalArguments);

                List<ClientTypeSignatureParameter> ConvertedArguments = [];

                // Talking to a legacy server (< 0.133)
                if (rawType.Equals(StandardTypes.ROW))
                {
                    int TypeArgSize = typeArguments.Count();
                    int LiteralArgSize = literalArguments.Count();

                    if (TypeArgSize != LiteralArgSize)
                    {
                        throw new ArgumentException($"The size of type arguments and literal arguments did not match: {TypeArgSize} && {LiteralArgSize}");
                    }

                    int Counter = 0;

                    foreach (object Item in literalArguments)
                    {
                        if (Item is not string)
                        {
                            throw new ArgumentException($"Expected literal argument {Item}, {Counter} in literalArguments to be a string.");
                        }

                        ConvertedArguments.Add(new ClientTypeSignatureParameter(new TypeSignatureParameter(new NamedTypeSignature(Item.ToString(), ToTypeSignature(typeArguments.ElementAt(Counter))))));

                        Counter++;
                    }
                }
                else
                {
                    if (LiteralArguments.Any())
                    {
                        throw new ArgumentException("Unexpected literal arguments from legacy server.");
                    }

                    foreach (ClientTypeSignature TypeArgument in TypeArguments)
                    {
                        ConvertedArguments.Add(new ClientTypeSignatureParameter(ParameterKind.TYPE, TypeArgument));
                    }
                }

                Arguments = ConvertedArguments;
            }
        }

        #endregion

        #region Private Methods

        private static TypeSignature ToTypeSignature(ClientTypeSignature signature)
        {
            IEnumerable<TypeSignatureParameter> Parameters = signature.Arguments.Select(x => LegacyClientTypeSignatureParameterToTypeSignatureParameter(x));
            return new TypeSignature(signature.RawType, Parameters);
        }

        private static TypeSignatureParameter LegacyClientTypeSignatureParameterToTypeSignatureParameter(ClientTypeSignatureParameter parameter)
        {
            return parameter.Kind switch
            {
                ParameterKind.LONG => throw new ArgumentException("Unexpected long type literal returned by legacy server"),
                ParameterKind.TYPE => new TypeSignatureParameter(ToTypeSignature(parameter.GetTypeSignature())),
                ParameterKind.NAMED_TYPE => new TypeSignatureParameter(parameter.GetNamedTypeSignature()),
                _ => throw new ArgumentException($"Unknown parameter kind {parameter.Kind}."),
            };
        }

        #endregion
    }
}
