using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.PlanFragmentId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class PlanFragmentId
    {
        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PlanFragmentId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "The id cannot be null or empty");
            }

            Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Id;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Id);
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

            PlanFragmentId That = (PlanFragmentId)obj;

            if (!Id.Equals(That.Id))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
