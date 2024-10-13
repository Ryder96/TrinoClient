using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// Represents an exception that can be created from a failure reported to the
    /// client
    /// Partially taken from com.facebook.presto.client.FailureInfo.java (internal class FailureException)
    /// </summary>
    public class TrinoFailureException : TrinoException
    {
        #region Public Properties

        public string Type { get; }

        public override string StackTrace { get; }

        public FailureInfo Cause { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TrinoFailureException(string message, string type, IEnumerable<string> stack, FailureInfo cause) : base(message)
        {
            ArgumentNullException.ThrowIfNull(stack);

            Type = type;
            StackTrace = string.Join("\n", stack);
            Cause = cause ?? throw new ArgumentNullException(nameof(cause));
        }

        #endregion
    }
}
