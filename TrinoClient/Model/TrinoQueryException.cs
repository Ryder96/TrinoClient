using System;
using System.Net;
using TrinoClient.Model.Client;

namespace TrinoClient.Model
{
    public class TrinoQueryException : TrinoWebException
    {
        #region Public Properties

        public string SqlState { get; }

        public int ErrorCode { get; }

        public string ErrorName { get; }

        public string ErrorType { get; }

        public ErrorLocation ErrorLocation { get; }

        public FailureInfo FailureInfo { get; }

        #endregion

        #region Constructors

        public TrinoQueryException(QueryError error) : base(error.Message, HttpStatusCode.OK)
        {
            ArgumentNullException.ThrowIfNull(error);

            ErrorCode = error.ErrorCode;
            ErrorLocation = error.ErrorLocation;
            ErrorName = error.ErrorName;
            ErrorType = error.ErrorType;
            FailureInfo = error.FailureInfo;
        }

        public TrinoQueryException(QueryError error, HttpStatusCode code) : base(error.Message, code)
        {
            ArgumentNullException.ThrowIfNull(error);

            ErrorCode = error.ErrorCode;
            ErrorLocation = error.ErrorLocation;
            ErrorName = error.ErrorName;
            ErrorType = error.ErrorType;
            FailureInfo = error.FailureInfo;
        }

        #endregion
    }
}
