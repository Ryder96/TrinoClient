using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Domain.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.spi.predicate.Domain.java
                                 /// </summary>
    public sealed class Domain(IValueSet values, bool nullAllowed)
    {
        #region Public Properties

        public IValueSet Values { get; } = values ?? throw new ArgumentNullException(nameof(values));

        public bool NullAllowed { get; } = nullAllowed;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public IType GetPrestoType()
        {
            return Values.GetPrestoType();
        }

        public bool IsNone()
        {
            return Values.IsNone() && !NullAllowed;
        }

        public bool IsAll()
        {
            return Values.IsAll() && NullAllowed;
        }

        public bool IsSingleValue()
        {
            return !NullAllowed && Values.IsSingleValue();
        }

        public bool IsNullableSingleValue()
        {
            if (NullAllowed)
            {
                return Values.IsNone();
            }
            else
            {
                return Values.IsSingleValue();
            }
        }

        public bool IsOnlyNull()
        {
            return Values.IsNone() && NullAllowed;
        }

        public object GetSingleValue()
        {
            if (!IsSingleValue())
            {
                throw new InvalidOperationException("Domain is not a single value.");
            }

            return Values.GetSingleValue();
        }

        public object GetNullableSingleValue()
        {
            if (!IsNullableSingleValue())
            {
                throw new InvalidOperationException("Domain is not a nullable single value.");
            }

            if (NullAllowed)
            {
                return null;
            }
            else
            {
                return Values.GetSingleValue();
            }
        }

        public bool IncludesNullableValue(object value)
        {
            return value == null ? NullAllowed : Values.ContainsValue(value);
        }

        public bool Overlaps(Domain other)
        {
            CheckCompatibility(other);
            return !Intersect(other).IsNone();
        }

        public bool Contains(Domain other)
        {
            CheckCompatibility(other);
            return Union(other).Equals(this);
        }

        public Domain Intersect(Domain other)
        {
            CheckCompatibility(other);
            return new Domain(Values.Intersect(other.Values), NullAllowed && other.NullAllowed);
        }

        public Domain Union(Domain other)
        {
            CheckCompatibility(other);
            return new Domain(Values.Union(other.Values), NullAllowed || other.NullAllowed);
        }

        public static Domain Union(IEnumerable<Domain> domains)
        {
            if (!domains.Any())
            {
                throw new ArgumentException("Domains cannot be empty for union.");
            }

            if (domains.Count() == 1)
            {
                return domains.ElementAt(0);
            }

            bool NullAllowed = domains.Any(x => x.NullAllowed);
            IEnumerable<IValueSet> ValueSets = domains.Select(x => x.Values);
            IValueSet UnionedValues = ValueSets.First().Union(ValueSets.Skip(1));

            return new Domain(UnionedValues, NullAllowed);
        }

        public Domain Complement()
        {
            return new Domain(Values.Complement(), !NullAllowed);
        }

        public Domain Substract(Domain other)
        {
            CheckCompatibility(other);
            return new Domain(Values.Subtract(other.Values), NullAllowed && !other.NullAllowed);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Values, NullAllowed);
        }

        public string ToString(IConnectorSession session)
        {
            return $"[ {(NullAllowed ? "NULL, " : "")}{Values.ToString(session)} ]";
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

            Domain Other = (Domain)obj;

            return Object.Equals(Values, Other.Values) &&
                    NullAllowed == Other.NullAllowed;
        }

        /// <summary>
        ///  Reduces the number of discrete components in the Domain if there are too many.
        /// </summary>
        /// <returns></returns>
        public Domain Simplify()
        {
            IValueSet SimplifiedValueSet = Values.GetValuesProcessor().Transform<IValueSet>(ranges =>
            {
                if (ranges.GetOrderedRanges().Count() <= 32)
                {
                    return null;
                }

                return ValueSet.OfRanges(ranges.GetSpan());
            },
            discreteValues =>
            {
                if (discreteValues.GetValues().Count() <= 32)
                {
                    return null;
                }

                return ValueSet.All(Values.GetPrestoType());
            },
            allOrNone =>
            {
                return null;
            });

            if (SimplifiedValueSet == null)
            {
                SimplifiedValueSet = Values;
            }

            return Domain.Create(SimplifiedValueSet, NullAllowed);
        }

        #endregion

        #region Public Static Methods

        public static Domain Create(IValueSet values, bool nullAllowed)
        {
            return new Domain(values, nullAllowed);
        }

        public static Domain None(IType type)
        {
            return new Domain(ValueSet.None(type), false);
        }

        public static Domain All(IType type)
        {
            return new Domain(ValueSet.All(type), true);
        }

        public static Domain OnlyNull(IType type)
        {
            return new Domain(ValueSet.None(type), true);
        }

        public static Domain NotNull(IType type)
        {
            return new Domain(ValueSet.All(type), false);
        }

        public static Domain SingleValue(IType type, object value)
        {
            return new Domain(ValueSet.Of(type, value), false);
        }

        public static Domain MultipleValues(IType type, IEnumerable<object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentException("values", "Values cannot be null or empty.");
            }

            if (values.Count() == 1)
            {
                return SingleValue(type, values.ElementAt(0));
            }

            return new Domain(ValueSet.Of(type, values.ElementAt(0), values.Skip(1)), false);
        }

        #endregion

        #region Private Methods

        public void CheckCompatibility(Domain domain)
        {
            if (!GetPrestoType().Equals(domain.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched domain types: {GetPrestoType()} vs {domain.GetPrestoType()}.");
            }

            if (Values.GetType() != domain.Values.GetType())
            {
                throw new ArgumentException($"Mismatched domain value set types: {Values.GetType().Name} vs {domain.Values.GetType().Name}.");
            }
        }

        #endregion
    }
}
