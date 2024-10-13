using Newtonsoft.Json;
using System;

namespace TrinoClient.Model.Execution.Buffer
{
    /// <summary>
    /// From com.facebook.execution.buffer.BufferInfo.java
    /// </summary>
    public class BufferInfo
    {
        #region Public Properties

        public OutputBufferId BufferId { get; }

        public bool Finished { get; }

        public int BufferedPages { get; }

        public long PagesSent { get; }

        public PageBufferInfo PageBufferInfo { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public BufferInfo(OutputBufferId bufferId, bool finished, int bufferedPages, long pagesSent, PageBufferInfo pageBufferInfo)
        {
            if (bufferedPages < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferedPages), "The buffered pages cannot be less than 0.");
            }

            if (pagesSent < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pagesSent), "The pages sent cannot be less than 0.");
            }

            BufferId = bufferId ?? throw new ArgumentNullException(nameof(bufferId));
            PagesSent = pagesSent;
            PageBufferInfo = pageBufferInfo ?? throw new ArgumentNullException(nameof(pageBufferInfo));
            Finished = finished;
            BufferedPages = bufferedPages;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("bufferId", BufferId)
                .Add("finished", Finished)
                .Add("bufferedPages", BufferedPages)
                .Add("pagesSent", PagesSent)
                .Add("pageBufferInfo", PageBufferInfo)
                .ToString();
        }

        #endregion
    }
}
