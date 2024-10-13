using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Specification)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Specification)
                                 /// </summary>
    public class Specification(IEnumerable<Symbol> partitionBy, OrderingScheme orderingScheme)
    {
        #region Public Properties

        public IEnumerable<Symbol> PartitionBy { get; } = partitionBy ?? throw new ArgumentNullException(nameof(partitionBy));

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public OrderingScheme OrderingScheme { get; } = orderingScheme;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return Hashing.Hash(OrderingScheme, PartitionBy);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Specification) != obj.GetType())
            {
                return false;
            }

            Specification other = (Specification)obj;

            return PartitionBy.Equals(other.PartitionBy) &&
                     Object.Equals(OrderingScheme, other.OrderingScheme);
        }

        #endregion
    }
}
