using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model
{
    /// <summary>
    /// From com.facebook.presto.OutputBuffers.java (interal class OutputBufferId)
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class OutputBufferId
    {
        #region Public Properties

        public int Id { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an output buffer id from a string form of an int.
        /// </summary>
        /// <param name="id">The string representation of an int.</param>
        [JsonConstructor]
        public OutputBufferId(string id)
        {
            int Temp = int.Parse(id);

            if (Temp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id cannot be less than 0.");
            }

            Id = Temp;
        }

        public OutputBufferId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id cannot be less than 0.");
            }

            Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Id.ToString();
        }

        #endregion
    }
}
