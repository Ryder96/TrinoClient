using System;
using TrinoClient.Model.SPI.Block;

namespace TrinoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.spi.type.AbstractVariableWidthType.java
    /// </summary>
    public abstract class AbstractVariableWidthType(TypeSignature signature, System.Type type) : AbstractType(signature, type), IVariableWidthType
    {
        #region Private Fields

        private static readonly int EXPECTED_BYTES_PER_ENTRY = 32;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries, int expectedBytesPerEntry)
        {
            int MaxBlockSizeInBytes;

            if (blockBuilderStatus == null)
            {
                MaxBlockSizeInBytes = BlockBuilderStatus.DEFAULT_MAX_BLOCK_SIZE_IN_BYTES;
            }
            else
            {
                MaxBlockSizeInBytes = blockBuilderStatus.MaxBlockSizeInBytes;
            }

            int ExpectedBytes = Math.Min(expectedEntries * expectedBytesPerEntry, MaxBlockSizeInBytes);

            return new VariableWidthBlockBuilder(
                blockBuilderStatus,
                expectedBytesPerEntry == 0 ? expectedEntries : Math.Min(expectedEntries, MaxBlockSizeInBytes / expectedBytesPerEntry),
                ExpectedBytes
                );
        }

        public override IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries)
        {
            return CreateBlockBuilder(blockBuilderStatus, expectedEntries, EXPECTED_BYTES_PER_ENTRY);
        }

        #endregion
    }
}
