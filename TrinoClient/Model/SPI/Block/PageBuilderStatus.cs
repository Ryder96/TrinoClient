namespace TrinoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.PageBuilderStatus.java
    /// </summary>
    public class PageBuilderStatus(int maxPageSizeInBytes, int maxBlockSizeInBytes)
    {
        #region Private Fields

        private bool _Full;

        private int _CurrentSize;

        #endregion

        #region Public Fields

        public static readonly int DEFAULT_MAX_PAGE_SIZE_IN_BYTES = 1024 * 1024;

        #endregion

        #region Public Properties

        public int MaxBlockSizeInBytes { get; } = maxBlockSizeInBytes;

        public int MaxPageSizeInBytes { get; } = maxPageSizeInBytes;

        public bool Full
        {
            get
            {
                return _Full || SizeInBytes >= MaxPageSizeInBytes;
            }
            set
            {
                _Full = value;
            }
        }

        public int SizeInBytes
        {
            get
            {
                return _CurrentSize;
            }
        }

        #endregion

        #region Constructors

        public PageBuilderStatus() : this(DEFAULT_MAX_PAGE_SIZE_IN_BYTES, BlockBuilderStatus.DEFAULT_MAX_BLOCK_SIZE_IN_BYTES)
        {
        }

        #endregion

        #region Public Methods

        public BlockBuilderStatus CreateBlockBuilderStatus()
        {
            return new BlockBuilderStatus(this, MaxBlockSizeInBytes);
        }

        public void AddBytes(int bytes)
        {
            _CurrentSize += bytes;
        }

        public bool IsEmpty()
        {
            return _CurrentSize == 0;
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxSizeInBytes", MaxPageSizeInBytes)
                .Add("full", _Full)
                .Add("currentSize", _CurrentSize)
                .ToString();
        }

        #endregion
    }
}
