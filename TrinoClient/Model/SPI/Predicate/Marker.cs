using Newtonsoft.Json;
using System;
using System.Text;
using TrinoClient.Model.SPI.Block;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Marker.java
    /// </summary>
    public class Marker : IComparable<Marker>
    {
        #region Public Properties

        public IType Type { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IBlock ValueBlock { get; }

        public Bound Bound { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// LOWER UNBOUNDED is specified with an empty value and a ABOVE bound
        /// UPPER UNBOUNDED is specified with an empty value and a BELOW bound
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueBlock"></param>
        /// <param name="bound"></param>
        [JsonConstructor]
        public Marker(IType type, IBlock valueBlock, Bound bound)
        {
            ValueBlock = valueBlock ?? throw new ArgumentNullException(nameof(valueBlock));
            Bound = bound;
            Type = type;

            if (!Type.IsOrderable())
            {
                throw new ArgumentException("Type must be orderable");
            }

            if (ValueBlock == null && Bound == Bound.EXACTLY)
            {
                throw new ArgumentException("Cannot be equal to unbounded.");
            }

            if (ValueBlock != null && ValueBlock.GetPositionCount() != 1)
            {
                throw new ArgumentException("Value block should only have one position.");
            }
        }

        #endregion

        #region Public Static Methods

        public static Marker UpperUnbounded(IType type)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, null, Bound.BELOW);
        }

        public static Marker LowerUnbounded(IType type)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, null, Bound.ABOVE);
        }

        public static Marker Above(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.ABOVE);
        }

        public static Marker Exactly(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.EXACTLY);
        }

        public static Marker Below(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.BELOW);
        }

        public static Marker Min(Marker marker1, Marker marker2)
        {
            return marker1.CompareTo(marker2) <= 0 ? marker1 : marker2;
        }

        public static Marker Max(Marker marker1, Marker marker2)
        {
            return marker1.CompareTo(marker2) >= 0 ? marker1 : marker2;
        }

        #endregion

        #region Public Methods

        public object GetValue()
        {
            if (ValueBlock == null)
            {
                throw new InvalidOperationException("No value to get.");
            }

            return Utils.BlockToNativeValue(Type, ValueBlock);
        }

        public object GetPrintableValue(IConnectorSession session)
        {
            if (ValueBlock == null)
            {
                throw new InvalidOperationException("No value to get.");
            }

            return Type.GetObjectValue(session, ValueBlock, 0);
        }

        public int CompareTo(Marker other)
        {
            CheckTypeCompatibility(other);

            if (IsUpperUnbounded())
            {
                return other.IsUpperUnbounded() ? 0 : 1;
            }

            if (IsLowerUnbounded())
            {
                return other.IsLowerUnbounded() ? 0 : -1;
            }

            if (other.IsUpperUnbounded())
            {
                return -1;
            }

            if (other.IsLowerUnbounded())
            {
                return 1;
            }
            // INVARIANT: value and o.value not null

            int Compare = Type.CompareTo(ValueBlock, 0, other.ValueBlock, 0);

            if (Compare == 0)
            {
                if (Bound == other.Bound)
                {
                    return 0;
                }
                if (Bound == Bound.BELOW)
                {
                    return -1;
                }
                if (Bound == Bound.ABOVE)
                {
                    return 1;
                }

                // INVARIANT: bound == EXACTLY
                return (other.Bound == Bound.BELOW) ? 1 : -1;
            }

            return Compare;
        }

        public bool IsUpperUnbounded()
        {
            return ValueBlock == null && Bound == Bound.BELOW;
        }

        public bool IsLowerUnbounded()
        {
            return ValueBlock == null && Bound == Bound.ABOVE;
        }

        /// <summary>
        /// Adjacency is defined by two Markers being infinitesimally close to each other.
        /// This means they must share the same value and have adjacent Bounds.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAdjacent(Marker other)
        {
            if (IsUpperUnbounded() || IsLowerUnbounded() || other.IsUpperUnbounded() || other.IsLowerUnbounded())
            {
                return false;
            }

            if (Type.CompareTo(ValueBlock, 0, other.ValueBlock, 0) != 0)
            {
                return false;
            }

            return (Bound == Bound.EXACTLY && other.Bound != Bound.EXACTLY) ||
                   (Bound != Bound.EXACTLY && other.Bound == Bound.EXACTLY);
        }

        public Marker GreaterAdjacent()
        {
            if (ValueBlock == null)
            {
                throw new InvalidOperationException("No marker adjacent to unbounded.");
            }

            switch (Bound)
            {
                case Bound.BELOW:
                    {
                        return new Marker(Type, ValueBlock, Bound.EXACTLY);
                    }
                case Bound.EXACTLY:
                    {
                        return new Marker(Type, ValueBlock, Bound.ABOVE);
                    }
                case Bound.ABOVE:
                    {
                        throw new InvalidOperationException("No greater marker adjacent to an ABOVE bound.");
                    }
                default:
                    {
                        throw new InvalidOperationException($"Unsupported type: {Bound.ToString()}");
                    }
            }
        }

        public Marker LesserAdjacent()
        {
            if (ValueBlock == null)
            {
                throw new InvalidOperationException("No marker adjacent to unbounded.");
            }

            switch (Bound)
            {
                case Bound.BELOW:
                    {
                        throw new InvalidOperationException("No lesser marker adjacent to a BELOW bound.");

                    }
                case Bound.EXACTLY:
                    {
                        return new Marker(Type, ValueBlock, Bound.BELOW);
                    }
                case Bound.ABOVE:
                    {
                        return new Marker(Type, ValueBlock, Bound.EXACTLY);
                    }
                default:
                    {
                        throw new InvalidOperationException($"Unsupported type: {Bound.ToString()}");
                    }
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int Hash = Hashing.Hash(Type, Bound);

                if (ValueBlock != null)
                {

                    Hash = Hash * 31 + (int)Type.Hash(ValueBlock, 0);
                }

                return Hash;
            }
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
            Marker Other = (Marker)obj;

            return Object.Equals(Type, Other.Type)
                    && Object.Equals(Bound, Other.Bound)
                    && (ValueBlock != null == (Other.ValueBlock != null))
                    && (ValueBlock == null || Type.EqualTo(ValueBlock, 0, Other.ValueBlock, 0));
        }

        public string ToString(IConnectorSession session)
        {
            StringBuilder SB = new("{");

            SB.Append("type=").Append(Type.ToString());
            SB.Append(", value=");

            if (IsLowerUnbounded())
            {
                SB.Append("<min>");
            }
            else if (IsUpperUnbounded())
            {
                SB.Append("<max>");
            }
            else
            {
                SB.Append(GetPrintableValue(session));
            }

            SB.Append(", bound=").Append(Bound.ToString());

            SB.Append("}");

            return SB.ToString();
        }

        #endregion

        #region Private Methods

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Mismatched Marker types: {Type.ToString()} vs {marker.Type.ToString()}.");
            }
        }

        private static Marker Create(IType type, object value, Bound bound)
        {
            return new Marker(type, Utils.NativeValueToBlock(type, value), bound);
        }

        #endregion
    }
}
