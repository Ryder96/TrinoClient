using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Client;
using TrinoClient.Model.SPI;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.ExecutionFailureInfo.java
    /// </summary>
    public class ExecutionFailureInfo
    {
        #region Public Properties

        public string Type { get; }

        public string Message { get; }

        public ExecutionFailureInfo Cause { get; }

        public IEnumerable<ExecutionFailureInfo> Suppressed { get; }

        public IEnumerable<string> Stack { get; }

        public ErrorLocation ErrorLocation { get; }

        public ErrorCode ErrorCode { get; }

        public HostAddress RemoteHost { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ExecutionFailureInfo(
            string type,
            string message,
            ExecutionFailureInfo cause,
            IEnumerable<ExecutionFailureInfo> suppressed,
            IEnumerable<string> stack,
            ErrorLocation errorLocation,
            ErrorCode errorCode,
            HostAddress remoteHost
        )
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type), "The type cannot be null or empty.");
            }

            Type = type;
            Message = message;
            Cause = cause;
            Suppressed = suppressed ?? throw new ArgumentNullException(nameof(suppressed), "Suppressed cannot be null.");
            Stack = stack ?? throw new ArgumentNullException(nameof(stack), "Stack cannot be null.");
            ErrorLocation = errorLocation;
            ErrorCode = errorCode;
            RemoteHost = remoteHost;
        }

        #endregion
    }
}
