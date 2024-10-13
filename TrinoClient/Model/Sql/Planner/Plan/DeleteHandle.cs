using Newtonsoft.Json;
using System;
using TrinoClient.Model.Metadata;
using TrinoClient.Model.SPI;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableWriterNode.java (internal class DeleteHandle)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.TableWriterNode.java (internal class DeleteHandle)
                                 /// </summary>
    public class DeleteHandle(TableHandle handle, SchemaTableName schemaTableName) : WriterTarget
    {
        #region Public Properties

        public TableHandle Handle { get; } = handle ?? throw new ArgumentNullException(nameof(handle));

        public SchemaTableName SchemaTableName { get; } = schemaTableName ?? throw new ArgumentNullException(nameof(schemaTableName));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Handle.ToString();
        }

        #endregion
    }
}
