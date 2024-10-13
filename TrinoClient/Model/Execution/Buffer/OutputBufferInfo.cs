using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.presto.execution.buffer.OutputBufferInfo.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.execution.buffer.OutputBufferInfo.java
                                 /// </summary>
    public class OutputBufferInfo(
        string type,
        BufferState state,
        bool canAddBuffers,
        bool canAddPages,
        long totalBufferedBytes,
        long totalBufferedPages,
        long totalRowsSent,
        long totalPagesSent,
        IEnumerable<BufferInfo> buffers
            )
    {
        #region Public Properties

        public string Type { get; } = type;

        public BufferState State { get; } = state;

        public bool CanAddBuffers { get; } = canAddBuffers;

        public bool CanAddPages { get; } = canAddPages;

        public long TotalBufferedBytes { get; } = totalBufferedBytes;

        public long TotalBufferedPages { get; } = totalBufferedPages;

        public long TotalRowsSent { get; } = totalRowsSent;

        public long TotalPagesSent { get; } = totalPagesSent;

        public IEnumerable<BufferInfo> Buffers { get; } = buffers;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", Type)
                .Add("state", State)
                .Add("canAddBuffers", CanAddBuffers)
                .Add("canAddPages", CanAddPages)
                .Add("totalBufferedBytes", TotalBufferedBytes)
                .Add("totalBufferedPages", TotalBufferedPages)
                .Add("totalRowsSent", TotalRowsSent)
                .Add("totalPagesSent", TotalPagesSent)
                .Add("buffers", Buffers)
                .ToString();
        }

        #endregion
    }
}
