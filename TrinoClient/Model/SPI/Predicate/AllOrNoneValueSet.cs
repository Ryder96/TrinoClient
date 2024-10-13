using System;
using System.Collections.Generic;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.AllOrNoneValueSet.java
    /// </summary>
    public class AllOrNoneValueSet(IType type, bool all) : IValueSet, IAllOrNone, IValuesProcessor
    {
        #region Private Fields

        private bool _All = all;

        #endregion

        #region Public Methods

        public IType Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public bool IsAll()
        {
            return _All;
        }

        public bool IsNone()
        {
            return !_All;
        }

        public bool IsSingleValue()
        {
            return false;
        }

        public object GetSingleValue()
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(object value)
        {
            if (!value.GetType().Equals(Type.GetJavaType()))
            {
                throw new ArgumentException($"Value class {value.GetType().Name} does not match required type, {Type}.");
            }

            return _All;
        }

        public IAllOrNone GetAllOrNone()
        {
            return this;
        }

        public IValuesProcessor GetValuesProcessor()
        {
            return this;
        }

        public T Transform<T>(Func<IRanges, T> rangesFunction, Func<IDiscreteValues, T> valuesFunction, Func<IAllOrNone, T> allOrNoneFunction)
        {
            return allOrNoneFunction.Invoke(GetAllOrNone());
        }

        public void Consume(Consumer<IRanges> rangesConsumer, Consumer<IDiscreteValues> valuesConsumer, Consumer<IAllOrNone> allOrNoneConsumer)
        {
            allOrNoneConsumer.Accept(GetAllOrNone());
        }

        public static IValueSet CopyOf(IType type, IEnumerable<Range> values)
        {
            return new AllOrNoneValueSet(type, true);
        }

        public static IValueSet Of(IType type, object first, params object[] rest)
        {
            return SortedRangeSet.Of(type, first, rest);
        }

        public override string ToString()
        {
            return $"[{(_All ? "ALL" : "NONE")}]";
        }

        public IType GetPrestoType()
        {
            return Type;
        }

        public IValueSet Intersect(IValueSet other)
        {
            AllOrNoneValueSet OtherValueSet = CheckCompatibility(other);
            return new AllOrNoneValueSet(Type, _All && OtherValueSet._All);
        }

        public IValueSet Union(IValueSet other)
        {
            AllOrNoneValueSet OtherValueSet = CheckCompatibility(other);
            return new AllOrNoneValueSet(Type, _All || OtherValueSet._All);
        }

        public IValueSet Union(IEnumerable<IValueSet> valueSets)
        {
            IValueSet Current = this;

            foreach (IValueSet Set in valueSets)
            {
                Current = Current.Union(Set);
            }

            return Current;
        }

        public bool Overlaps(IValueSet other)
        {
            return !Intersect(other).IsNone();
        }

        public IValueSet Subtract(IValueSet other)
        {
            return Intersect(other.Complement());
        }

        public bool Contains(IValueSet other)
        {
            return Union(other).Equals(this);
        }

        public IValueSet Complement()
        {
            return new AllOrNoneValueSet(Type, !_All);
        }

        public string ToString(IConnectorSession session)
        {
            return $"[{(_All ? "ALL" : "NONE")}]";
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

            AllOrNoneValueSet Other = (AllOrNoneValueSet)obj;
            return Object.Equals(Type, Other.Type)
                    && _All == Other._All;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Type, _All);
        }

        #endregion

        #region Public Static Methods

        public static AllOrNoneValueSet All(IType type)
        {
            return new AllOrNoneValueSet(type, true);
        }

        public static AllOrNoneValueSet None(IType type)
        {
            return new AllOrNoneValueSet(type, false);
        }

        #endregion

        #region Private Methods

        private AllOrNoneValueSet CheckCompatibility(IValueSet other)
        {
            if (Type.Equals(other.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched types: {Type} vs {other.GetPrestoType()}.");
            }

            if (other is not AllOrNoneValueSet)
            {
                throw new ArgumentException($"ValueSet is not a AllOrNoneValueSet: {other.GetType().Name}.");
            }

            return (AllOrNoneValueSet)other;
        }

        #endregion
    }
}
