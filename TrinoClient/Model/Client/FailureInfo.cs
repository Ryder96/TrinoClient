using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.FailureInfo.java
    /// </summary>
    public class FailureInfo
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; }

        public string Message { get; }

        public FailureInfo Cause { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<FailureInfo> Suppressed { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Stack { get; }

        public ErrorLocation ErrorLocation { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public FailureInfo(
            string type,
            string message,
            FailureInfo cause,
            IEnumerable<FailureInfo> suppressed,
            IEnumerable<string> stack,
            ErrorLocation errorLocation
            )
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
            Message = message;
            Cause = cause;
            Suppressed = suppressed ?? throw new ArgumentNullException(nameof(suppressed));
            Stack = stack;
            ErrorLocation = errorLocation;
        }

        #endregion

        #region Public Methods

        public TrinoException ToException()
        {
            return ToException(this);
        }

        public TrinoFailureException ToPrestoFailureException()
        {
            return ToException(this);
        }

        private static TrinoFailureException ToException(FailureInfo failureInfo)
        {
            if (failureInfo == null)
            {
                return null;
            }

            return new TrinoFailureException(
                failureInfo.Message,
                failureInfo.Type,
                failureInfo.Stack,
                failureInfo.Cause
            );
        }

        #endregion
    }
}
