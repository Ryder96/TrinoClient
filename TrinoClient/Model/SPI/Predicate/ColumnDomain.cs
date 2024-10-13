using Newtonsoft.Json;
using System;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.TupleDomain.java (internal class ColumnDomain)
    /// 
    /// Used for serialization only
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(ColumnDomainConverter<>))]
    [method: JsonConstructor]
    public class ColumnDomain<T>(T column, Domain domain)
    {
        #region Public Properties

        public T Column { get; } = column;

        public Domain Domain { get; } = domain ?? throw new ArgumentNullException(nameof(domain));

        #endregion
        #region Constructors

        #endregion
    }
}
