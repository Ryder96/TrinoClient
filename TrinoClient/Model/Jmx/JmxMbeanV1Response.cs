using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Jmx
{
    /// <summary>
    /// A response containg jmx mbean details
    /// </summary>
    public class JmxMbeanV1Response
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public JmxMbeanV1Result Response { get; }

        /// <summary>
        /// Indicates whether deserialization was successful.
        /// </summary>
        public bool DeserializationSucceeded { get; }

        /// <summary>
        /// If deserialization fails, the will contain the thrown exception. Otherwise, 
        /// this property is null.
        /// </summary>
        public Exception LastError { get; }

        #endregion

        #region Constructors

        public JmxMbeanV1Response(string rawContent)
        {
            RawContent = rawContent;

            if (!string.IsNullOrEmpty(RawContent))
            {
                try
                {
                    Response = JsonConvert.DeserializeObject<JmxMbeanV1Result>(RawContent);
                    DeserializationSucceeded = true;
                    LastError = null;
                }
                catch (Exception e)
                {
                    DeserializationSucceeded = false;
                    LastError = e;
                    Response = null;
                }
            }
        }

        #endregion
    }
}
