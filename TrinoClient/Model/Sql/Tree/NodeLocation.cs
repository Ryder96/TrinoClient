using Newtonsoft.Json;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.NodeLocation.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.NodeLocation.java
                                 /// </summary>
    public sealed class NodeLocation(int line, int columnNumber)
    {
        #region Private Fields

        private int CharPositionInLine = columnNumber - 1;

        #endregion

        #region Public Properties

        public int Line { get; } = line;

        public int ColumnNumber
        {
            get
            {
                return CharPositionInLine + 1;
            }
        }

        #endregion
        #region Constructors

        #endregion
    }
}
