using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model
{
    [JsonConverter(typeof(DataSizeConverter))]
    public class DataSize : IComparable<DataSize>
    {
        #region Public Properties

        public double Size { get; }

        public DataSizeUnit Unit { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DataSize(double size, DataSizeUnit unit)
        {
            if (double.IsInfinity(size))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "The size is infinity.");
            }

            if (double.IsNaN(size))
            {
                throw new ArgumentOutOfRangeException(nameof(size), "The size is NaN.");
            }

            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "The size is less than 0.");
            }

            Size = size;
            Unit = unit;
        }

        #endregion

        #region Public Methods

        public static DataSize SuccinctBytes(long bytes)
        {
            return SuccinctDataSize(bytes, DataSizeUnit.BYTE);
        }

        public static DataSize SuccinctDataSize(double size, DataSizeUnit unit)
        {
            return new DataSize(size, unit).ConvertToMostSuccinctDataSize();
        }

        public DataSize ConvertToMostSuccinctDataSize()
        {
            DataSizeUnit UnitToUse = DataSizeUnit.BYTE;

            foreach (DataSizeUnit UnitToTest in Enum.GetValues(typeof(DataSizeUnit)))
            {
                if (GetValue(UnitToTest) >= 1.0)
                {
                    UnitToUse = UnitToTest;
                }
                else
                {
                    break;
                }
            }

            return ConvertTo(UnitToUse);
        }

        public DataSize ConvertTo(DataSizeUnit unit)
        {
            return new DataSize(GetValue(unit), unit);
        }

        public override string ToString()
        {
            return $"{Size.ToString("0.##")}{Unit.GetUnitString()}";
        }

        public double GetValue(DataSizeUnit unit)
        {
            return Size * (Unit.GetFactor() * (1.0 / unit.GetFactor()));
        }

        public int CompareTo(DataSize other)
        {
            return GetValue(DataSizeUnit.BYTE).CompareTo(other.GetValue(DataSizeUnit.BYTE));
        }

        public long ToBytes()
        {
            double Bytes = GetValue(DataSizeUnit.BYTE);

            ParameterCheck.Check(Bytes <= long.MaxValue, "Size in bytes is too large to be represented in bytes as a long.");

            return (long)Bytes;
        }

        #endregion

    }
}
