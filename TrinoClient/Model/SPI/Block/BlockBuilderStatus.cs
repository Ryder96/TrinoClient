using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TrinoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.BlockBuilderStatus.java
    /// </summary>
    public class BlockBuilderStatus(PageBuilderStatus pageBuilderStatus, int maxBlockSizeInBytes)
    {
        #region Private Fields

        private PageBuilderStatus _PageBuilderStatus = pageBuilderStatus ?? throw new ArgumentNullException(nameof(pageBuilderStatus));

        private int _CurrentSize;

        #endregion

        #region Public Properties

        public int MaxBlockSizeInBytes { get; } = maxBlockSizeInBytes;

        #endregion

        #region Public Fields

        public static readonly int INSTANCE_SIZE = DeepInstanceSize(typeof(BlockBuilderStatus));

        public static readonly int DEFAULT_MAX_BLOCK_SIZE_IN_BYTES = 64 * 1024;

        #endregion

        #region Constructors

        public BlockBuilderStatus() : this(new PageBuilderStatus(PageBuilderStatus.DEFAULT_MAX_PAGE_SIZE_IN_BYTES, DEFAULT_MAX_BLOCK_SIZE_IN_BYTES), DEFAULT_MAX_BLOCK_SIZE_IN_BYTES)
        {
        }

        #endregion

        #region Private Methods

        private static int DeepInstanceSize(System.Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (type.IsArray)
            {
                throw new ArgumentException($"Cannot determine size of {type.Name} because it contains an array.");
            }

            if (type.IsInterface)
            {
                throw new ArgumentException($"{type.Name} is an interface.");
            }

            if (type.IsAbstract)
            {
                throw new ArgumentException($"{type.Name} is abstract.");
            }

            if (!type.BaseType.Equals(typeof(object)))
            {
                throw new ArgumentException($"Cannot determine size of a subclass. {type.Name} extends from {type.BaseType.Name}.");
            }

            int Size = Marshal.SizeOf(type);

            foreach (PropertyInfo Info in type.GetProperties())
            {
                if (!Info.PropertyType.IsPrimitive)
                {
                    Size += DeepInstanceSize(Info.PropertyType);
                }
            }

            return Size;
        }

        #endregion

        #region Public Methods

        public void AddBytes(int bytes)
        {
            _CurrentSize += bytes;
            _PageBuilderStatus.AddBytes(bytes);

            if (_CurrentSize >= MaxBlockSizeInBytes)
            {
                _PageBuilderStatus.Full = true;
            }
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxSizeInBytes", MaxBlockSizeInBytes)
                .Add("currentSize", _CurrentSize)
                .ToString();
        }

        #endregion
    }
}
