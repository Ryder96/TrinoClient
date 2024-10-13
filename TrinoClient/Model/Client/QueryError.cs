using Newtonsoft.Json;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.QueryError.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.client.QueryError.java
                                 /// </summary>
    public class QueryError(string message, string sqlState, int errorCode, string errorName, string errorType, ErrorLocation errorLocation, FailureInfo failureInfo)
    {
        #region Public Properties

        public string Message { get; } = message;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SqlState { get; } = sqlState;

        public int ErrorCode { get; } = errorCode;

        public string ErrorName { get; } = errorName;

        public string ErrorType { get; } = errorType;

        public ErrorLocation ErrorLocation { get; } = errorLocation;

        public FailureInfo FailureInfo { get; } = failureInfo;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("message", Message)
                .Add("sqlState", SqlState)
                .Add("errorCode", ErrorCode)
                .Add("errorName", ErrorName)
                .Add("errorType", ErrorType)
                .Add("errorLocation", ErrorLocation)
                .Add("failureInfo", FailureInfo)
                .ToString();
        }

        #endregion
    }
}
