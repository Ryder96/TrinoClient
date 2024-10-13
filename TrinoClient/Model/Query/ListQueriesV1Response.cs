using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Server;

namespace TrinoClient.Model.Query
{
    /// <summary>
    /// This is the response object returned to the client when making a
    /// request for a list of query info
    /// </summary>
    public class ListQueriesV1Response
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public IEnumerable<BasicQueryInfo> QueryInfo { get; }

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

        /// <summary>
        /// Creates a new response from the JSON array string returned from presto.
        /// </summary>
        /// <param name="rawContent">The JSON array of query information</param>
        internal ListQueriesV1Response(string rawContent)
        {
            RawContent = rawContent;

            if (!string.IsNullOrEmpty(RawContent))
            {
                try
                {
                    QueryInfo = JsonConvert.DeserializeObject<IEnumerable<BasicQueryInfo>>(RawContent);
                    DeserializationSucceeded = true;
                    LastError = null;
                }
                catch (Exception e)
                {
                    DeserializationSucceeded = false;
                    LastError = e;
                    QueryInfo = null;
                }
            }
        }

        #endregion
    }
}
