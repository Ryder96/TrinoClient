using System;

namespace TrinoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.VarcharType.java
    /// </summary>
    public sealed class VarcharType : AbstractVariableWidthType
    {
        #region Private Fields

        private int _Length;

        #endregion

        #region Public Properties

        public static readonly int UNBOUNDED_LENGTH = int.MaxValue;
        public static readonly int MAX_LENGTH = int.MaxValue - 1;
        public static readonly VarcharType VARCHAR = new(UNBOUNDED_LENGTH);

        public int Length
        {
            get
            {
                if (IsUnbounded())
                {
                    throw new InvalidOperationException("Cannot get size of unbounded VARCHAR.");
                }

                return _Length;
            }
        }

        #endregion

        #region Constructors

        private VarcharType(int length) : base(new TypeSignature(StandardTypes.VARCHAR, new TypeSignatureParameter((long)length)), typeof(string))
        {
            ParameterCheck.OutOfRange(length >= 0, "length");

            _Length = length;
        }

        #endregion

        #region Public Methods

        public bool IsUnbounded()
        {
            return _Length == UNBOUNDED_LENGTH;
        }

        public override bool IsComparable()
        {
            return true;
        }

        public override bool IsOrderable()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            VarcharType other = (VarcharType)obj;

            return Object.Equals(_Length, other._Length);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(_Length);
        }

        public string DisplayName()
        {
            if (_Length == UNBOUNDED_LENGTH)
            {
                return Signature.Base;
            }

            return Signature.ToString();
        }

        public override string ToString()
        {
            return DisplayName();
        }

        #endregion
    }
}
