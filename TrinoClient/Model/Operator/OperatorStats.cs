using Newtonsoft.Json;
using System;
using TrinoClient.Model.Execution.Scheduler;
using TrinoClient.Model.Sql.Planner.Plan;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.OperatorStats.java
    /// </summary>
    public class OperatorStats
    {
        #region Public Properties

        public int PipelineId { get; }

        public int OperatorId { get; }

        public PlanNodeId PlanNodeId { get; }

        public string OperatorType { get; }

        public long TotalDrivers { get; }

        public long AddInputCalls { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputWall { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputCpu { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AddInputUser { get; }

        public DataSize InputDataSize { get; }

        public long InputPositions { get; }

        public double SumSquaredInputPositions { get; }

        public long GetOutputCalls { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputWall { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputCpu { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan GetOutputUser { get; }

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan BlockedWall { get; }

        public long FinishCalls { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishWall { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishCpu { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishUser { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize RevocableMemoryReservation { get; }

        public DataSize SystemMemoryReservation { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public BlockedReason BlockedReason { get; }

        /// <summary>
        /// This property is only present with certain types of
        /// operator summaries, like a TableScanOperator. The value takes on many different
        /// forms in the serialized Json, for example:
        /// 
        /// "info": {
        ///   "@type": "splitOperator",
        ///   "splitInfo": {
        ///     "path": "s3a://filename.avro",
        ///     "start": 0,
        ///     "length": 218,
        ///     "fileSize": 218,
        ///     "hosts": [ "localhost" ],
        ///     "database": "db",
        ///     "table": "test",
        ///     "forceLocalScheduling": false,
        ///     "partitionName": "<UNPARTITIONED>"
        ///   }
        /// }
        /// 
        /// As well as
        /// "info": {
        ///   "@type": "splitOperator",
        ///   "splitInfo": {
        ///     "@type": "$info_schema",
        ///     "tableHandle": {
        ///       "@type": "$info_schema",
        ///       "catalogName": "hive",
        ///       "schemaName": "information_schema",
        ///       "tableName": "tables"
        ///     },
        ///     "filters": {
        ///       "table_schema": {
        ///         "serializable": {
        ///           "type": "varchar",
        ///           "block": "DgAAAFZBUklBQkxFX1dJRFRIAQAAAAUAAAAABQAAAG1hdmVu"
        ///         }
        ///       }
        ///     },
        ///     "addresses": [ "172.25.0.3:8080" ]
        ///   }
        /// }
        /// 
        /// Thus, this property is deserialized as dynamic.
        /// 
        /// TODO: The actual property type is IOperatorInfo
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Info { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public OperatorStats(
            int pipelineId,
            int operatorId,
            PlanNodeId planNodeId,
            string operatorType,

            long totalDrivers,

            long addInputCalls,
            TimeSpan addInputWall,
            TimeSpan addInputCpu,
            TimeSpan addInputUser,
            DataSize inputDataSize,
            long inputPositions,
            double sumSquaredInputPositions,

            long getOuputCalls,
            TimeSpan getOutputWall,
            TimeSpan getOutputCpu,
            TimeSpan getOutputUser,
            DataSize outputDataSize,
            long outputPositions,

            DataSize physicalWrittenDataSize,

            TimeSpan blockedWall,

            long finishCalls,
            TimeSpan finishWall,
            TimeSpan finishCpu,
            TimeSpan finishUser,

            DataSize userMemoryReservation,
            DataSize revocableMemoryReservation,
            DataSize systemMemoryReservation,
            BlockedReason blockedReason,

            dynamic info
            )
        {
            ParameterCheck.OutOfRange(operatorId >= 0, "Operator id cannot be negative.");
            ParameterCheck.NotNullOrEmpty(operatorType, "operatorType");
            ParameterCheck.OutOfRange(inputPositions >= 0, "inputPositions", "Input positions cannot be negative.");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions", "Output positions cannot be negative.");

            OperatorId = operatorId;
            PlanNodeId = planNodeId ?? throw new ArgumentNullException(nameof(planNodeId));
            OperatorType = operatorType;

            TotalDrivers = totalDrivers;

            AddInputCalls = addInputCalls;
            AddInputWall = addInputWall;
            AddInputCpu = addInputCpu;
            AddInputUser = addInputUser;
            InputDataSize = inputDataSize ?? throw new ArgumentNullException(nameof(inputDataSize));
            InputPositions = inputPositions;
            SumSquaredInputPositions = sumSquaredInputPositions;

            GetOutputCalls = getOuputCalls;
            GetOutputWall = getOutputWall;
            GetOutputCpu = getOutputCpu;
            GetOutputUser = getOutputUser;
            OutputDataSize = outputDataSize ?? throw new ArgumentNullException(nameof(outputDataSize));
            OutputPositions = outputPositions;

            PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException(nameof(physicalWrittenDataSize));

            BlockedWall = blockedWall;

            FinishCalls = finishCalls;
            FinishWall = finishWall;
            FinishCpu = finishCpu;
            FinishUser = finishUser;

            UserMemoryReservation = userMemoryReservation ?? throw new ArgumentNullException(nameof(userMemoryReservation));
            RevocableMemoryReservation = revocableMemoryReservation ?? throw new ArgumentNullException(nameof(revocableMemoryReservation));
            SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException(nameof(systemMemoryReservation));
            BlockedReason = blockedReason;

            Info = info;
        }

        #endregion

        #region Public Methods

        public OperatorStats Summarize()
        {
            return new OperatorStats(
                    PipelineId,
                    OperatorId,
                    PlanNodeId,
                    OperatorType,
                    TotalDrivers,
                    AddInputCalls,
                    AddInputWall,
                    AddInputCpu,
                    AddInputUser,
                    InputDataSize,
                    InputPositions,
                    SumSquaredInputPositions,
                    GetOutputCalls,
                    GetOutputWall,
                    GetOutputCpu,
                    GetOutputUser,
                    OutputDataSize,
                    OutputPositions,
                    PhysicalWrittenDataSize,
                    BlockedWall,
                    FinishCalls,
                    FinishWall,
                    FinishCpu,
                    FinishUser,
                    UserMemoryReservation,
                    RevocableMemoryReservation,
                    SystemMemoryReservation,
                    BlockedReason,
                    (Info != null /* && this.Info.IsFinal() */) ? Info : null);
        }

        #endregion
    }
}
