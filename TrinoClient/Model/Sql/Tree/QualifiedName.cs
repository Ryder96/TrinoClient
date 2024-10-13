using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrinoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.QualifiedName.java
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.sql.tree.QualifiedName.java
                                 /// </summary>
    public class QualifiedName(IEnumerable<string> originalParts, IEnumerable<string> parts)
    {
        #region Public Properties

        public IEnumerable<string> Parts { get; } = parts;

        public IEnumerable<string> OriginalParts { get; } = originalParts;

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Join(".", Parts);
        }

        public QualifiedName GetPrefix()
        {
            if (Parts.Count() == 1)
            {
                return null;
            }
            else
            {
                IEnumerable<string> SubList = Parts.Take(Parts.Count() - 1);
                return new QualifiedName(SubList, SubList);
            }
        }

        #endregion
    }
}
