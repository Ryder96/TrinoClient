using Newtonsoft.Json;

namespace TrinoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.presto.execution.buffer.PageBufferInfo.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.execution.buffer.PageBufferInfo.java
                                 /// </summary>
    public class PageBufferInfo(int partition, long bufferedPages, long bufferedBytes, long rowsAdded, long pagesAdded)
    {
        #region Public Properties

        public int Partition { get; } = partition;

        public long BufferedPages { get; } = bufferedPages;

        public long BufferedBytes { get; } = bufferedBytes;

        public long RowsAdded { get; } = rowsAdded;

        public long PagesAdded { get; } = pagesAdded;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("partition", Partition)
                .Add("bufferedPages", BufferedPages)
                .Add("bufferedBytes", BufferedBytes)
                .Add("rowsAdded", RowsAdded)
                .Add("pagesAdded", PagesAdded)
                .ToString();
        }

        #endregion
    }
}
