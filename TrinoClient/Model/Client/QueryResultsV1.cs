using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrinoClient.Model.Client;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Statement
{
    public class QueryResultsV1 : QueryResults, IQueryData, IQueryStatusInfo
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri PartialCancelUri { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<List<dynamic>> Data { get; set; }

        public StatementStats Stats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long UpdateCount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for query results
        /// </summary>
        /// <param name="id"></param>
        /// <param name="infoUri"></param>
        /// <param name="partialCancelUri"></param>
        /// <param name="nextUri"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <param name="stats"></param>
        /// <param name="error"></param>
        /// <param name="updateType"></param>
        /// <param name="updateCount"></param>
        [JsonConstructor]
        public QueryResultsV1(
            string id,
            Uri infoUri,
            Uri partialCancelUri,
            Uri nextUri,
            IEnumerable<Column> columns,
            IEnumerable<List<dynamic>> data,
            StatementStats stats,
            QueryError error,
            string updateType,
            long updateCount
            ) : base(id, infoUri, nextUri, columns, error)
        {
            if (data != null && columns == null)
            {
                throw new ArgumentException("Data present without columns");
            }

            PartialCancelUri = partialCancelUri;
            Data = data;
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
            UpdateType = updateType;
            UpdateCount = updateCount;
        }

        #endregion

        #region Public Methods

        #region IQueryData

        /// <summary>
        /// Returns the data from the query
        /// </summary>
        /// <returns>Any available data from this query result</returns>
        public IEnumerable<List<dynamic>> GetData()
        {
            return Data;
        }

        #endregion

        #region IQueryStatus

        public string GetId()
        {
            return Id;
        }

        public Uri GetInfoUri()
        {
            return InfoUri;
        }

        public Uri GetPartialCancelUri()
        {
            return PartialCancelUri;
        }

        public Uri GetNextUri()
        {
            return NextUri;
        }

        public IEnumerable<Column> GetColumns()
        {
            return Columns;
        }

        public StatementStats GetStats()
        {
            return Stats;
        }

        public QueryError GetError()
        {
            return Error;
        }

        public string GetUpdateType()
        {
            return UpdateType;
        }

        public long GetUpdateCount()
        {
            return UpdateCount;
        }

        #endregion

        /// <summary>
        /// Converts the data rows to a CSV formatted array, each line
        /// in the array is CSV data and can be combined with a String.Join("\n", csvArray)
        /// function to get a single CSV string
        /// </summary>
        /// <returns>The data formatted as CSV with 1 line per index in the IEnumerable</returns>
        public IEnumerable<string> DataToCsv()
        {
            if (Data != null)
            {
                foreach (var Item in Data)
                {
                    StringBuilder SB = new();

                    foreach (var Column in Item)
                    {
                        SB.Append($"\"{Column}\",");
                    }

                    // Remove last comma
                    SB.Length--;

                    yield return SB.ToString();
                }
            }
            else
            {
                throw new ArgumentNullException("Data", "The data in this query result is null.");
            }
        }

        /// <summary>
        /// Converts the data rows into a JSON string using a Dictionary type mapping
        /// </summary>
        /// <returns>The JSON formatted string</returns>
        public string DataToJson()
        {
            if (Data != null && Columns != null)
            {
                Dictionary<string, Dictionary<string, object>[]> Wrapper = new()
                {
                    { "data", new Dictionary<string, object>[Data.Count()] }
                };

                Column[] Columns = this.Columns.ToArray();
                int RowCounter = 0;

                foreach (List<dynamic> Row in Data)
                {
                    // Keep track of the column number
                    int Counter = 0;

                    Wrapper["data"][RowCounter] = [];

                    foreach (dynamic Column in Row)
                    {
                        Column Col = Columns[Counter++];

                        object Value = TrinoTypeMapping.Convert(Column.ToString(), Col.TypeSignature.RawType);

                        Wrapper["data"][RowCounter].Add(Col.Name, Value);
                    }

                    RowCounter++;
                }

                return JsonConvert.SerializeObject(Wrapper);
            }
            else
            {
                return null;
            }
        }


        public static bool TryParse(string content, out QueryResultsV1 result, out Exception ex)
        {
            ex = null;

            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<QueryResultsV1>(content);
                    return true;
                }
                catch (Exception e)
                {
                    result = null;
                    ex = e;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }


        #endregion
    }
}
