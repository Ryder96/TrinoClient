using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TrinoClient.Model.Client;

namespace TrinoClient.Model.Statement
{
    [method: JsonConstructor]
    public class QueryResultsV2(
        string id,
        Uri infoUri,
        Uri finalUri,
        Uri nextUri,
        bool nextUriDone,
        IEnumerable<Column> columns,
        IEnumerable<Uri> dataUris,
        Actions actions,
        QueryError error
            ) : QueryResults(id, infoUri, nextUri, columns, error), IQueryData
    {
        #region Public Properties

        public bool NextUriDone { get; } = nextUriDone;

        public Uri FinalUri { get; } = finalUri;

        public IEnumerable<Uri> DataUris { get; } = dataUris;

        public Actions Actions { get; } = actions;

        public IEnumerable<List<dynamic>> Data { get; set; }

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public IEnumerable<List<dynamic>> GetData()
        {
            return Data;
        }

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

        #endregion
    }
}
