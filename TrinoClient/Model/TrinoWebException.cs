using System;
using System.Net;

namespace TrinoClient.Model
{
    /// <summary>
    /// Represents an exception that occurs during a web API call to Presto
    /// </summary>
    public class TrinoWebException : TrinoException
    {
        #region Public Properties

        /// <summary>
        /// The return status code of the request that failed
        /// </summary>
        public HttpStatusCode StatusCode { get; }


        #endregion

        #region Constructors

        public TrinoWebException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public TrinoWebException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public TrinoWebException(string message, string rawContent, HttpStatusCode statusCode) : base(message, rawContent)
        {
            StatusCode = statusCode;
        }

        public TrinoWebException(string message, string rawContent, HttpStatusCode statusCode, Exception innerException) : base(message, rawContent, innerException)
        {
            StatusCode = statusCode;
        }
    }

    #endregion
}
