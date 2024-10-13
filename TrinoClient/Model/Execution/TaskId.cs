using Newtonsoft.Json;
using System;
using TrinoClient.Model.SPI;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class TaskId
    {
        #region Public Properties

        public string FullId { get; }

        #endregion

        #region Constructors

        public TaskId(string queryId, int stageId, int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id cannot be less than 0.");
            }

            FullId = $"{queryId}.{stageId}.{id}";
        }

        public TaskId(StageId stageId, int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id cannot be less than 0.");
            }

            FullId = $"{stageId.QueryId.Id}.{stageId.Id}.{id}";
        }

        [JsonConstructor]
        public TaskId(string fullId)
        {
            FullId = fullId;
        }

        #endregion

        #region Public Methods

        public QueryId GetQueryId()
        {
            return new QueryId(FullId.Split('.')[0]);
        }

        public StageId GetStageId()
        {
            return new StageId(FullId.Split('.')[1]);
        }

        public int GetId()
        {
            return int.Parse(FullId.Split('.')[2]);
        }

        public override string ToString()
        {
            return FullId;
        }

        #endregion
    }
}
