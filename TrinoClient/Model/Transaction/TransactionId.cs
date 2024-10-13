using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Transaction
{
    /// <summary>
    /// From com.facebook.transaction.TransactionId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class TransactionId
    {
        #region Public Properties

        public Guid UUID { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TransactionId(Guid uuid)
        {
            UUID = uuid;
        }

        public TransactionId(string uuid)
        {
            UUID = Guid.Parse(uuid);
        }

        #endregion

        #region Public Methods

        public static TransactionId Create()
        {
            return new TransactionId(Guid.NewGuid());
        }

        public static TransactionId ValueOf(string value)
        {
            return new TransactionId(Guid.Parse(value));
        }

        public override string ToString()
        {
            return UUID.ToString();
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(UUID);
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

            TransactionId Other = (TransactionId)obj;

            return UUID == Other.UUID;
        }

        #endregion
    }
}
