using System;

namespace TrinoClient.Model.Jmx
{
    /// <summary>
    /// A request for statistics on a jmx mbean
    /// </summary>
    public class JmxMbeanV1Request
    {
        #region Public Properties

        /// <summary>
        /// The name of the object to request statistics for
        /// </summary>
        public string ObjectName { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new request for the given object name
        /// </summary>
        /// <param name="objectName"></param>
        public JmxMbeanV1Request(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException(nameof(objectName), "The object name cannot be null or empty.");
            }

            ObjectName = objectName;
        }

        #endregion
    }
}
