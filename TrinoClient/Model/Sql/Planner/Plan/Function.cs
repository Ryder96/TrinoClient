using Newtonsoft.Json;
using System;
using TrinoClient.Model.Metadata;
using TrinoClient.Model.Sql.Tree;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Function)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Function)
                                 /// </summary>
    public class Function(FunctionCall functionCall, Signature signature, Frame frame)
    {
        #region Public Properties

        public FunctionCall FunctionCall { get; } = functionCall ?? throw new ArgumentNullException(nameof(functionCall));

        public Signature Signature { get; } = signature ?? throw new ArgumentNullException(nameof(signature));

        public Frame Frame { get; } = frame ?? throw new ArgumentNullException(nameof(frame));

        #endregion
        #region Constructors

        #endregion

        #region Public Properties

        public override int GetHashCode()
        {
            return Hashing.Hash(FunctionCall, Signature, Frame);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Function) != obj.GetType())
            {
                return false;
            }

            Function other = (Function)obj;

            return object.Equals(FunctionCall, other.FunctionCall) &&
                    object.Equals(Signature, other.Signature) &&
                    object.Equals(Frame, other.Frame);
        }

        #endregion
    }
}
