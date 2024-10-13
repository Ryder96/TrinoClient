using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrinoClient.Model.SPI.Type
{
    public class TypeSignature
    {
        #region Private Fields

        private static Dictionary<string, string> BASE_NAME_ALIAS_TO_CANONICAL = [];

        static TypeSignature()
        {
            BASE_NAME_ALIAS_TO_CANONICAL.Add("int", StandardTypes.INTEGER);
        }

        #endregion

        #region Public Properties

        public string Base { get; }

        public IEnumerable<TypeSignatureParameter> Parameters { get; }

        public bool Calculated { get; }

        #endregion

        #region Constructors

        public TypeSignature(string @base, params TypeSignatureParameter[] parameters) : this(@base, parameters.ToList())
        {
        }

        [JsonConstructor]
        public TypeSignature(string @base, IEnumerable<TypeSignatureParameter> parameters)
        {
            ParameterCheck.NotNullOrEmpty(@base, "base");

            Base = @base;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Calculated = parameters.Any(x => x.IsCalculated());
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (Base.Equals(StandardTypes.ROW))
            {
                return "";
            }
            else if (Base.Equals(StandardTypes.VARCHAR) &&
                Parameters.Count() == 1 &&
                Parameters.ElementAt(0).IsLongLiteral() &&
                Parameters.ElementAt(0).GetLongLiteral() == VarcharType.UNBOUNDED_LENGTH
                )
            {
                return Base;
            }
            else
            {
                StringBuilder TypeName = new(Base);

                if (Parameters.Any())
                {
                    TypeName.Append($"({string.Join(",", Parameters.Select(x => x.ToString()))})");
                }

                return TypeName.ToString();
            }
        }

        public IEnumerable<TypeSignature> GetTypeParametersAsTypeSignatures()
        {
            foreach (TypeSignatureParameter Parameter in Parameters)
            {
                if (Parameter.Kind != ParameterKind.TYPE)
                {
                    throw new FormatException($"Expected all parameters to be TypeSignatures but {Parameter.ToString()} was found");
                }

                yield return Parameter.GetTypeSignature();
            }
        }

        #endregion
    }
}
