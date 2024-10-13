using System;
using System.Text;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.Range.java
    /// 
    /// A Range of values across the continuous space defined by the types of the Markers
    /// </summary>
    public sealed class Range
    {
        #region Public Properties

        public Marker Low { get; }

        public Marker High { get; }

        public IType Type
        {
            get
            {
                return Low.Type;
            }
        }

        #endregion

        #region Constructors

        public Range(Marker low, Marker high)
        {
            ArgumentNullException.ThrowIfNull(low);

            ArgumentNullException.ThrowIfNull(high);

            if (low.IsUpperUnbounded())
            {
                throw new ArgumentException("Low cannot be upper unbounded.");
            }

            if (high.IsLowerUnbounded())
            {
                throw new ArgumentException("High cannot be lower unbounded.");
            }

            if (low.CompareTo(high) > 0)
            {
                throw new ArgumentOutOfRangeException("Low must be less than or equal to high.");
            }

            Low = low;
            High = high;
        }

        #endregion

        #region Public Methods

        public bool IsSingleValue()
        {
            return Low.Bound == Bound.EXACTLY && Low.Equals(High);
        }

        public object GetSingleValue()
        {
            if (!IsSingleValue())
            {
                throw new InvalidOperationException("Range does not have just a single value.");
            }

            return Low.GetValue();
        }

        public bool IsAll()
        {
            return Low.IsLowerUnbounded() && High.IsUpperUnbounded();
        }

        public bool Includes(Marker marker)
        {
            ArgumentNullException.ThrowIfNull(marker);

            CheckTypeCompatibility(marker);

            return Low.CompareTo(marker) <= 0 && High.CompareTo(marker) >= 0;
        }

        public bool Contains(Range other)
        {
            CheckTypeCompatibility(other);

            return Low.CompareTo(other.Low) <= 0 &&
                   High.CompareTo(other.High) >= 0;
        }

        public Range Span(Range other)
        {
            CheckTypeCompatibility(other);

            Marker LowMarker = Marker.Min(Low, other.Low);
            Marker HighMarker = Marker.Max(High, other.High);
            return new Range(LowMarker, HighMarker);
        }

        public bool Overlaps(Range other)
        {
            CheckTypeCompatibility(other);

            return Low.CompareTo(other.High) <= 0 &&
                other.Low.CompareTo(High) <= 0;
        }

        public Range Intersect(Range other)
        {
            CheckTypeCompatibility(other);

            if (!Overlaps(other))
            {
                throw new ArgumentException("Cannot intersect non-overlapping ranges");
            }

            Marker LowMarker = Marker.Max(Low, other.Low);
            Marker HighMarker = Marker.Min(High, other.High);
            return new Range(LowMarker, HighMarker);
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

            Range Other = (Range)obj;

            return Low.Equals(Other.Low) &&
                    High.Equals(Other.High);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Low, High);
        }

        public string ToString(IConnectorSession session)
        {
            StringBuilder SB = new();

            if (IsSingleValue())
            {
                SB.Append("[").Append(Low.GetPrintableValue(session)).Append("]");
            }
            else
            {
                SB.Append(Low.Bound == Bound.EXACTLY ? "[" : "(");
                SB.Append(Low.IsLowerUnbounded() ? "<min>" : Low.GetPrintableValue(session));
                SB.Append(", ");
                SB.Append(High.IsUpperUnbounded() ? "<max>" : High.GetPrintableValue(session));
                SB.Append(High.Bound == Bound.EXACTLY ? "]" : ")");
            }

            return SB.ToString();
        }

        #endregion

        #region Public Static Methods

        public static Range All(IType type)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.UpperUnbounded(type));
        }

        public static Range GreaterThan(IType type, object low)
        {
            return new Range(Marker.Above(type, low), Marker.UpperUnbounded(type));
        }

        public static Range GreaterThanOrEqual(IType type, object low)
        {
            return new Range(Marker.Exactly(type, low), Marker.UpperUnbounded(type));
        }

        public static Range LessThan(IType type, object high)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.Below(type, high));
        }

        public static Range LessThanOrEqualTo(IType type, object high)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.Exactly(type, high));
        }

        public static Range Equal(IType type, object value)
        {
            return new Range(Marker.Exactly(type, value), Marker.Exactly(type, value));
        }

        public static Range Create(IType type, object low, bool lowInclusive, object high, bool highInclusive)
        {
            Marker LowMarker = lowInclusive ? Marker.Exactly(type, low) : Marker.Above(type, low);
            Marker HighMarker = highInclusive ? Marker.Exactly(type, high) : Marker.Below(type, high);
            return new Range(LowMarker, HighMarker);
        }

        #endregion

        #region Private Methods

        private void CheckTypeCompatibility(Range range)
        {
            if (!Type.Equals(range.Type))
            {
                throw new ArgumentException($"Mismatched Range types: {Type} vs {range.Type}.");
            }
        }

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Marker of {marker.Type} does not match Range of {Type}.");
            }
        }

        #endregion
    }
}
