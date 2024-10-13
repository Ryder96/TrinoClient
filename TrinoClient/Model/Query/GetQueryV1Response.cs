using Newtonsoft.Json;
using System;
using TrinoClient.Model.Execution;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Query
{
    /// <summary>
    /// A response object for a request of the details of a specific query.
    /// </summary>
    public class GetQueryV1Response
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public QueryInfo QueryInfo { get; }

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
        /// Creates a new response from the JSON object string returned from presto.
        /// </summary>
        /// <param name="rawContent">The JSON object of query details</param>
        internal GetQueryV1Response(string rawContent)
        {
            RawContent = rawContent;

            if (!string.IsNullOrEmpty(RawContent))
            {
                try
                {
                    JsonConverter[] Converters = [new PlanNodeConverter()];
                    QueryInfo = JsonConvert.DeserializeObject<QueryInfo>(RawContent, new JsonSerializerSettings() { Converters = Converters });
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
