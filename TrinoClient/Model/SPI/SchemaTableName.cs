using Newtonsoft.Json;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.SchemaTableName.java
    /// </summary>
    public class SchemaTableName
    {
        #region Public Properties

        public string SchemaName { get; }

        public string TableName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SchemaTableName(string schemaName, string tableName)
        {
            ParameterCheck.NotNullOrEmpty(schemaName, "schemaName");
            ParameterCheck.NotNullOrEmpty(tableName, "tableName");

            SchemaName = schemaName.ToLower();
            TableName = tableName.ToLower();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{SchemaName}.{TableName}";
        }

        #endregion
    }
}
