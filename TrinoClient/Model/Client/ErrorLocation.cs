using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ErrorLocation.java
    /// </summary>
    public class ErrorLocation
    {
        #region Public Properties

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        #endregion

        #region Constructors 

        [JsonConstructor]
        public ErrorLocation(int lineNumber, int columnNumber)
        {
            if (lineNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNumber), "The line number must be at least one.");
            }

            if (columnNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnNumber), "The column number must be at least one.");
            }

            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("lineNumber", LineNumber)
                .Add("columnNumber", ColumnNumber)
                .ToString();
        }

        #endregion
    }
}
