﻿using Newtonsoft.Json;
using System;
using TrinoClient.Model.Client;

namespace TrinoClient.Model.Statement
{
    public class ExecuteQueryResponse<T> where T : QueryResults
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public T Response { get; }

        /// <summary>
        /// Indicates whether deserialization was successful.
        /// </summary>
        public bool DeserializationSucceeded { get; }

        /// <summary>
        /// Indicates whether the query was successfully closed by the client.
        /// </summary>
        public bool QueryClosed { get; internal set; }

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
        internal ExecuteQueryResponse(string rawContent, bool closed = false)
        {
            RawContent = rawContent;

            if (!string.IsNullOrEmpty(RawContent))
            {
                try
                {
                    Response = JsonConvert.DeserializeObject<T>(RawContent);
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

            QueryClosed = closed;
        }

        #endregion
    }
}
