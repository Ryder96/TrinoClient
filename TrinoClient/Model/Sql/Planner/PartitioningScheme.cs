using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PartitioningScheme.java
    /// </summary>
    public class PartitioningScheme
    {
        #region Public Properties

        public Partitioning Partitioning { get; }

        public IEnumerable<Symbol> OutputLayout { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashColumn { get; }

        public bool ReplicateNullsAndAny { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IEnumerable<int> BucketToPartition { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PartitioningScheme(
            Partitioning partitioning,
            IEnumerable<Symbol> outputLayout,
            Symbol hashColumn,
            bool replicateNullsAndAny,
            int[] bucketToPartition
            )
        {
            Partitioning = partitioning ?? throw new ArgumentNullException(nameof(partitioning));
            OutputLayout = outputLayout ?? throw new ArgumentNullException(nameof(outputLayout));

            HashSet<Symbol> Columns = Partitioning.GetColumns();

            ParameterCheck.Check(Columns.All(x => OutputLayout.Contains(x)), $"Output layout ({OutputLayout}) doesn't include all partition colums ({Columns}).");

            HashColumn = hashColumn;

            ParameterCheck.Check(!replicateNullsAndAny || Columns.Count <= 1, "Must have at most one partitioning column when nullPartition is REPLICATE.");
            ReplicateNullsAndAny = replicateNullsAndAny;
            BucketToPartition = bucketToPartition;
        }

        #endregion

        #region Public Methods

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

            PartitioningScheme That = (PartitioningScheme)obj;

            return Object.Equals(Partitioning, That.Partitioning) &&
                    Object.Equals(OutputLayout, That.OutputLayout) &&
                    ReplicateNullsAndAny == That.ReplicateNullsAndAny &&
                    Object.Equals(BucketToPartition, That.BucketToPartition);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Partitioning, OutputLayout, ReplicateNullsAndAny, BucketToPartition);
        }

        public override string ToString()
        {

            return StringHelper.Build(this)
                    .Add("partitioning", Partitioning)
                    .Add("outputLayout", OutputLayout)
                    .Add("hashChannel", HashColumn)
                    .Add("replicateNullsAndAny", ReplicateNullsAndAny)
                    .Add("bucketToPartition", BucketToPartition)
                    .ToString();
        }

        #endregion

    }
}
