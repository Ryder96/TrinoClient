using System;
using System.Collections.Generic;
using System.Linq;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.SortedRangeSet.java
    /// </summary>
    public sealed class SortedRangeSet : IValueSet, IRanges, IValuesProcessor
    {
        #region Public Properties

        public IType Type { get; }

        public SortedDictionary<Marker, Range> LowIndexedRanges { get; }

        public IEnumerable<Range> Ranges
        {
            get
            {
                return LowIndexedRanges.Values;
            }
        }

        #endregion

        #region Constructors

        public SortedRangeSet(IType type, SortedDictionary<Marker, Range> lowIndexedRanges)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            LowIndexedRanges = lowIndexedRanges ?? throw new ArgumentNullException(nameof(lowIndexedRanges));

            if (!Type.IsOrderable())
            {
                throw new ArgumentException($"Type is not orderable: {Type.ToString()}.", nameof(type));
            }

        }

        #endregion

        #region Public Static Methods

        public static SortedRangeSet None(IType type)
        {
            return CopyOf(type, []);
        }

        public static SortedRangeSet All(IType type)
        {
            return CopyOf(type, [Range.All(type)]);
        }

        /// <summary>
        /// Provided Ranges are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static SortedRangeSet CopyOf(IType type, IEnumerable<Range> ranges)
        {
            ranges = ranges.OrderBy(x => x.Low);

            Range Current = null;

            SortedDictionary<Marker, Range> Result = [];

            foreach (Range Next in ranges)
            {
                if (Current == null)
                {
                    Current = Next;
                    continue;
                }

                if (Current.Overlaps(Next) || Current.High.IsAdjacent(Next.Low))
                {
                    Current = Current.Span(Next);
                }
                else
                {
                    Result.Add(Current.Low, Current);
                }
            }

            if (Current != null)
            {
                Result.Add(Current.Low, Current);
            }

            return new SortedRangeSet(type, Result);
        }

        /// <summary>
        /// Provided discrete values that are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="type"></param>
        /// <param name="first"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public static SortedRangeSet Of(IType type, object first, params object[] rest)
        {
            return CopyOf(type, rest.Concat([first]).Select(x => Range.Equal(type, x)));
        }

        /// <summary>
        /// Provided Ranges are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="first"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public static SortedRangeSet Of(Range first, params Range[] rest)
        {
            return CopyOf(first.Type, rest.Concat([first]));
        }

        #endregion

        #region Public Methods

        public bool IsNone()
        {
            return LowIndexedRanges.Count == 0;
        }

        public bool IsAll()
        {
            return LowIndexedRanges.Count == 1 && LowIndexedRanges.Values.First().IsAll();
        }

        public bool IsSingleValue()
        {
            return LowIndexedRanges.Count == 1 && LowIndexedRanges.Values.First().IsSingleValue();
        }

        public object GetSingleValue()
        {
            if (!IsSingleValue())
            {
                throw new IndexOutOfRangeException("SortedRAngeSet does not have just a single value.");
            }

            return LowIndexedRanges.Values.First().GetSingleValue();
        }

        public bool ContainsValue(object value)
        {
            return IncludesMarker(Marker.Exactly(Type, value));
        }

        public IRanges GetRanges()
        {
            return (IRanges)this;
        }

        public IValuesProcessor GetValuesProcessor()
        {
            return this;
        }

        public T Transform<T>(Func<IRanges, T> rangesFunction, Func<IDiscreteValues, T> valuesFunction, Func<IAllOrNone, T> allOrNoneFunction)
        {
            return rangesFunction.Invoke(GetRanges());
        }

        public void Consume(Consumer<IRanges> rangesConsumer, Consumer<IDiscreteValues> valuesConsumer, Consumer<IAllOrNone> allOrNoneConsumer)
        {
            rangesConsumer.Accept(GetRanges());
        }

        public int GetRangeCount()
        {
            return LowIndexedRanges.Count;
        }

        public IEnumerable<Range> GetOrderedRanges()
        {
            return Ranges;
        }

        public Range GetSpan()
        {
            if (LowIndexedRanges.Count == 0)
            {
                throw new InvalidOperationException("Cannot get span if no ranges exists");
            }

            return LowIndexedRanges.First().Value.Span(LowIndexedRanges.Last().Value);
        }

        public IType GetPrestoType()
        {
            return Type;
        }

        public IValueSet Intersect(IValueSet other)
        {
            SortedRangeSet OtherRangeSet = CheckCompatibility(other);

            Builder Builder = new(Type);

            IEnumerator<Range> Iterator1 = GetOrderedRanges().GetEnumerator();
            IEnumerator<Range> Iterator2 = OtherRangeSet.GetOrderedRanges().GetEnumerator();

            if (Iterator1.MoveNext() && Iterator2.MoveNext())
            {
                Range Range1 = Iterator1.Current;
                Range Range2 = Iterator2.Current;

                while (true)
                {
                    if (Range1.Overlaps(Range2))
                    {
                        Builder.Add(Range1.Intersect(Range2));
                    }

                    if (Range1.High.CompareTo(Range2.High) <= 0)
                    {
                        if (!Iterator1.MoveNext())
                        {
                            break;
                        }

                        Range1 = Iterator1.Current;
                    }
                    else
                    {
                        if (!Iterator2.MoveNext())
                        {
                            break;
                        }

                        Range2 = Iterator2.Current;
                    }
                }
            }

            return Builder.Build();
        }

        public IValueSet Union(IValueSet other)
        {
            SortedRangeSet OtherRangeSet = CheckCompatibility(other);

            return new Builder(Type)
                .AddAll(GetOrderedRanges())
                .AddAll(OtherRangeSet.GetOrderedRanges())
                .Build();
        }

        public IValueSet Union(IEnumerable<IValueSet> valueSets)
        {
            Builder Builder = new(Type);
            Builder.AddAll(GetOrderedRanges());

            foreach (IValueSet Set in valueSets)
            {
                Builder.AddAll(CheckCompatibility(Set).GetOrderedRanges());
            }

            return Builder.Build();
        }

        public IValueSet Complement()
        {
            Builder Builder = new(Type);

            if (LowIndexedRanges.Count == 0)
            {
                return Builder.Add(Range.All(Type)).Build();
            }

            IEnumerable<Range> RangeIterator = LowIndexedRanges.Values;

            Range FirstRange = RangeIterator.First();

            if (!FirstRange.Low.IsLowerUnbounded())
            {
                Builder.Add(new Range(Marker.LowerUnbounded(Type), FirstRange.Low.LesserAdjacent()));
            }

            Range PreviousRange = FirstRange;

            foreach (Range Next in RangeIterator.Skip(1))
            {
                Marker LowMarker = PreviousRange.High.GreaterAdjacent();
                Marker HighMarker = Next.Low.LesserAdjacent();

                Builder.Add(new Range(LowMarker, HighMarker));

                PreviousRange = Next;
            }

            Range LastRange = PreviousRange;

            if (!LastRange.High.IsUpperUnbounded())
            {
                Builder.Add(new Range(LastRange.High.GreaterAdjacent(), Marker.UpperUnbounded(Type)));
            }

            return Builder.Build();
        }

        public bool Overlaps(IValueSet other)
        {
            return !Intersect(other).IsNone();
        }

        public bool Contains(IValueSet other)
        {
            return Union(other).Equals(this);
        }

        public IValueSet Subtract(IValueSet other)
        {
            return Intersect(other.Complement());
        }

        public string ToString(IConnectorSession session)
        {
            return $"[{string.Join(", ", LowIndexedRanges.Values.Select(x => x.ToString(session)))}]";
        }

        public bool IncludesMarker(Marker marker)
        {
            KeyValuePair<Marker, Range> FloorEntry = LowIndexedRanges.FloorEntry(marker);
            return !FloorEntry.Equals(default(KeyValuePair<Marker, Range>)) && FloorEntry.Value.Includes(marker);
        }

        #endregion

        #region Private Methods

        private SortedRangeSet CheckCompatibility(IValueSet other)
        {
            if (!Type.Equals(other.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched types: {Type} vs {other.GetPrestoType()}.");
            }

            if (other is not SortedRangeSet)
            {
                throw new ArgumentException($"ValueSet is not a SortedRangeSet: {other.GetType()}.");
            }

            return (SortedRangeSet)other;
        }

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Marker of {marker.Type} does not match SortedRangeSet of {Type}.");
            }
        }


        #endregion

        #region Internal Classes

        internal class Builder
        {
            private IType Type { get; }

            private List<Range> Ranges { get; }

            internal Builder(IType type)
            {
                Type = type ?? throw new ArgumentNullException(nameof(type));

                if (!Type.IsOrderable())
                {
                    throw new ArgumentException($"Type is not orderable: {Type.ToString()}.");
                }

                Ranges = [];
            }

            internal Builder Add(Range range)
            {
                if (!Type.Equals(range.Type))
                {
                    throw new ArgumentException($"Range type {range.Type.ToString()} does not match builder type {Type.ToString()}.");
                }

                Ranges.Add(range);
                return this;
            }

            internal Builder AddAll(IEnumerable<Range> ranges)
            {
                Ranges.AddRange(ranges);

                return this;
            }

            internal SortedRangeSet Build()
            {
                Ranges.Sort(new Comparison<Range>(delegate (Range x, Range y)
                {
                    return x.Low.CompareTo(y.Low);
                }));

                SortedDictionary<Marker, Range> Result = [];

                Range Current = null;

                foreach (Range Next in Ranges)
                {
                    if (Current == null)
                    {
                        Current = Next;
                        continue;
                    }

                    if (Current.Overlaps(Next) || Current.High.IsAdjacent(Next.Low))
                    {
                        Current = Current.Span(Next);
                    }
                    else
                    {
                        Result.Add(Current.Low, Current);
                    }
                }

                if (Current != null)
                {
                    Result.Add(Current.Low, Current);
                }

                return new SortedRangeSet(Type, Result);
            }
        }

        #endregion
    }
}
