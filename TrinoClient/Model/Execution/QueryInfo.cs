using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TrinoClient.Model.Client;
using TrinoClient.Model.SPI;
using TrinoClient.Model.SPI.Memory;
using TrinoClient.Model.Transaction;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.QueryInfo.java
    /// </summary>
    public class QueryInfo
    {
        #region Public Properties

        public QueryId QueryId { get; }

        public SessionRepresentation Session { get; }

        public QueryState State { get; }

        public MemoryPoolId MemoryPool { get; }

        public bool Scheduled { get; }

        public Uri Self { get; }

        public IEnumerable<string> FieldNames { get; }

        public string Query { get; }

        public QueryStats QueryStats { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetCatalog { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetSchema { get; }

        public IDictionary<string, string> SetSessionProperties { get; }

        public HashSet<string> ResetSessionProperties { get; }

        public IDictionary<string, string> AddedPreparedStatements { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<string> DeallocatedPreparedStatements { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TransactionId StartedTransactionId { get; }

        public bool ClearTransactionId { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string UpdateType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public StageInfo OutputStage { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FailureInfo FailureInfo { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorType ErrorType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorCode ErrorCode { get; }

        public bool FinalQueryInfo { get; }

        public HashSet<Input> Inputs { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Output Output { get; }

        public bool CompleteInfo { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string ResourceGroupName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryInfo(
            QueryId queryId,
            SessionRepresentation session,
            QueryState state,
            MemoryPoolId memoryPool,
            bool scheduled,
            Uri self,
            IEnumerable<string> fieldNames,
            string query,
            QueryStats queryStats,
            string setCatalog,
            string setSchema,
            IDictionary<string, string> setSessionProperties,
            HashSet<string> resetSessionProperties,
            IDictionary<string, string> addedPreparedStatements,
            HashSet<string> deallocatedPreparedStatemetns,
            TransactionId startedTransactionId,
            bool clearTransactionId,
            string updateType,
            StageInfo outputStage,
            FailureInfo failureInfo,
            ErrorCode errorCode,
            HashSet<Input> inputs,
            Output output,
            bool completeInfo,
            string resourceGroupName
            )
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            QueryId = queryId ?? throw new ArgumentNullException(nameof(queryId));
            Session = session ?? throw new ArgumentNullException(nameof(session));
            State = state;
            MemoryPool = memoryPool ?? throw new ArgumentNullException(nameof(memoryPool));
            Scheduled = scheduled;
            Self = self ?? throw new ArgumentNullException(nameof(self));
            FieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));
            Query = query;
            QueryStats = queryStats ?? throw new ArgumentNullException(nameof(queryStats));
            SetCatalog = setCatalog;
            SetSchema = setSchema;
            SetSessionProperties = setSessionProperties ?? throw new ArgumentNullException(nameof(setSessionProperties));
            ResetSessionProperties = resetSessionProperties ?? throw new ArgumentNullException(nameof(resetSessionProperties));
            AddedPreparedStatements = addedPreparedStatements ?? throw new ArgumentNullException(nameof(addedPreparedStatements));
            DeallocatedPreparedStatements = deallocatedPreparedStatemetns; // ?? throw new ArgumentNullException("deallocatedPreparedStatements");
            StartedTransactionId = startedTransactionId;
            ClearTransactionId = clearTransactionId;
            UpdateType = updateType;
            OutputStage = outputStage;
            FailureInfo = failureInfo;
            ErrorType = errorCode == null ? ErrorType.NONE : errorCode.Type;
            ErrorCode = errorCode;
            Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
            Output = output;
            CompleteInfo = completeInfo;
            ResourceGroupName = resourceGroupName;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("queryId", QueryId)
                .Add("state", State)
                .Add("fieldNames", FieldNames)
                .ToString();
        }

        #endregion
    }
}
