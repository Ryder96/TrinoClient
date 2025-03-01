﻿using TrinoClient.Model.FailureDetector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.NodeInfo
{
    /// <summary>
    /// A response from a request to list the nodes in the cluster
    /// </summary>
    public class ListNodesV1Response
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public IEnumerable<HeartbeatFailureDetectorStats> Nodes { get; set; }

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
        /// <param name="rawContent">The JSON array of nodes</param>
        internal ListNodesV1Response(string rawContent)
        {
            this.RawContent = rawContent;

            if (!String.IsNullOrEmpty(this.RawContent))
            {
                try
                {
                    this.Nodes = JsonConvert.DeserializeObject<IEnumerable<HeartbeatFailureDetectorStats>>(this.RawContent);
                    this.DeserializationSucceeded = true;
                    this.LastError = null;
                }
                catch (Exception e)
                {
                    this.DeserializationSucceeded = false;
                    this.LastError = e;
                    this.Nodes = null;
                }
            }
        }

        #endregion
    }
}
