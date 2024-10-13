using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.Signature.java
    /// </summary>
    public class Signature
    {
        #region Public Properties

        public string Name { get; }

        public FunctionKind Kind { get; }

        public IEnumerable<TypeVariableConstraint> TypeVariableConstraints { get; }

        public IEnumerable<LongVariableConstraint> LongVariableConstraints { get; }

        public TypeSignature ReturnType { get; }

        public IEnumerable<TypeSignature> ArgumentTypes { get; }

        public bool VariableArity { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Signature(
            string name,
            FunctionKind kind,
            IEnumerable<TypeVariableConstraint> typeVariableConstraints,
            IEnumerable<LongVariableConstraint> longVariableConstraints,
            TypeSignature returnType,
            IEnumerable<TypeSignature> argumentTypes,
            bool variableArity)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Kind = kind;
            TypeVariableConstraints = typeVariableConstraints;
            LongVariableConstraints = longVariableConstraints;
            ReturnType = returnType;
            ArgumentTypes = argumentTypes;
            VariableArity = variableArity;
        }

        #endregion
    }
}
