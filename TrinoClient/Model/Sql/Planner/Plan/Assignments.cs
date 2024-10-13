using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.Assignments.java
    /// </summary>
    public class Assignments(IDictionary<string, dynamic> assignments)
    {
        #region Public Properties

        /// <summary>
        /// TODO: Key should be <Symbol, Expression>
        /// </summary>
        public IDictionary<string, dynamic> assignments { get; } = assignments ?? throw new ArgumentNullException(nameof(assignments));

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public IEnumerable<Symbol> GetOutputs()
        {
            return assignments.Keys.Select(x => new Symbol(x));
        }

        public IEnumerable<dynamic> GetExpressions()
        {
            return assignments.Values;
        }

        public HashSet<Symbol> GetSymbols()
        {
            return new HashSet<Symbol>(assignments.Keys.Select(x => new Symbol(x)));
        }

        public int Size()
        {
            return assignments.Count;
        }

        #endregion
    }
}
