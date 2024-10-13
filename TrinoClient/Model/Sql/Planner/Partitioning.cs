using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Partitioning.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.planner.Partitioning.java
                                 /// </summary>
    public class Partitioning(PartitioningHandle handle, IEnumerable<ArgumentBinding> arguments)
    {
        #region Public Properties

        public PartitioningHandle Handle { get; } = handle ?? throw new ArgumentNullException(nameof(handle));

        public IEnumerable<ArgumentBinding> Arguments { get; } = arguments ?? throw new ArgumentNullException(nameof(arguments));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public HashSet<Symbol> GetColumns()
        {
            return new HashSet<Symbol>(Arguments.Where(x => x.IsVariable()).Select(x => x.Column));
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("handle", Handle)
                .Add("arguments", Arguments)
                .ToString();
        }

        #endregion
    }
}
