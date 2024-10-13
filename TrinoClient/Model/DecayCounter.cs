using Newtonsoft.Json;
using System;

namespace TrinoClient.Model
{
    /// <summary>
    /// A counter that decays exponentially. Values are weighted according to the formula
    /// w(t, α) = e^(-α * t), where α is the decay factor and t is the age in seconds
    /// 
    /// The implementation is based on the ideas from
    /// http://www.research.att.com/people/Cormode_Graham/library/publications/CormodeShkapenyukSrivastavaXu09.pdf
    /// to not have to rely on a timer that decays the value periodically
    /// 
    /// From io.airlift.stats.DecayCounter.java
    /// </summary>
    public class DecayCounter(double alpha)
    {
        #region Private Fields

        // needs to be such that Math.exp(alpha * seconds) does not grow too big
        private static readonly long RESCALE_THRESHOLD_SECONDS = 50;

        private volatile object SyncRoot = new();

        private double Count;

        private long LandmarkInSeconds = GetTickInSeconds();

        #endregion

        #region Public Properties

        public double Alpha { get; } = alpha;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public void Add(long value)
        {
            lock (SyncRoot)
            {
                long NowInSeconds = GetTickInSeconds();

                if (NowInSeconds - LandmarkInSeconds >= RESCALE_THRESHOLD_SECONDS)
                {
                    RescaleToNewLandmark(NowInSeconds);
                }

                Count += value * Weight(NowInSeconds, LandmarkInSeconds);
            }
        }

        public void Merge(DecayCounter decayCounter)
        {
            ArgumentNullException.ThrowIfNull(decayCounter);

            ParameterCheck.Check(decayCounter.Alpha == Alpha, $"Expected decayCounter to have alpha {Alpha}, but was {decayCounter.Alpha}.");

            lock (SyncRoot)
            {
                // if the landmark this counter is behind the other counter
                if (LandmarkInSeconds < decayCounter.LandmarkInSeconds)
                {
                    // rescale this counter to the other counter, and add
                    RescaleToNewLandmark(decayCounter.LandmarkInSeconds);
                    Count += decayCounter.Count;
                }
                else
                {
                    // rescale the other counter and add
                    double OtherRescaledCount = decayCounter.Count / Weight(LandmarkInSeconds, decayCounter.LandmarkInSeconds);
                    Count += OtherRescaledCount;
                }
            }
        }

        public void Reset()
        {
            lock (SyncRoot)
            {
                LandmarkInSeconds = GetTickInSeconds();
                Count = 0;
            }
        }

        public double GetCount()
        {
            lock (SyncRoot)
            {
                long NowInSeconds = GetTickInSeconds();
                return Count / Weight(NowInSeconds, LandmarkInSeconds);
            }
        }

        public double GetRate()
        {
            lock (SyncRoot)
            {
                return GetCount() * Alpha;
            }
        }

        public DecayCounterSnapshot Snapshot()
        {
            return new DecayCounterSnapshot(GetCount(), GetRate());
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("count", GetCount())
                .Add("rate", GetRate())
                .ToString();
        }

        #endregion

        #region Private Methods

        private static long GetTickInSeconds()
        {
            return DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
        }

        private void RescaleToNewLandmark(long newLandMarkInSeconds)
        {
            // rescale the count based on a new landmark to avoid numerical overflow issues
            Count /= Weight(newLandMarkInSeconds, LandmarkInSeconds);
            LandmarkInSeconds = newLandMarkInSeconds;
        }

        private double Weight(long timestampInSeconds, long landmarkInSeconds)
        {
            return Math.Exp(Alpha * (timestampInSeconds - landmarkInSeconds));
        }

        #endregion

        #region Internal Classes

        [method: JsonConstructor]
        #endregion

        #region Internal Classes

        public class DecayCounterSnapshot(double count, double rate)
        {
            #region Public Methods

            public double Count { get; } = count;

            public double Rate { get; } = rate;

            #endregion
            #region Constructors

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return StringHelper.Build(this)
                    .Add("count", Count)
                    .Add("rate", Rate)
                    .ToString();
            }

            #endregion
        }

        #endregion
    }
}
