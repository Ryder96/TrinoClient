using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrinoClient.Model.Server
{
    /// <summary>
    /// A Java thread resource.
    /// From com.facebook.presto.server.ThreadResource.java
    /// 
    /// This class is really just a data holder since the actual implementation
    /// creates and populates the thread info
    /// </summary>
    public class ThreadResource
    {
        #region Public Properties

        public IEnumerable<Info> ThreadInfo { get; private set; }

        #endregion

        #region Constructors

        private ThreadResource()
        { }

        #endregion

        #region Internal Classes

        [method: JsonConstructor]
        #endregion

        #region Internal Classes

        public class Info(long id, string name, ThreadState state, long lockOwner, IEnumerable<StackLine> stackTrace)
        {
            #region Public Properties

            public long Id { get; } = id;

            public string Name { get; } = name;

            public ThreadState State { get; } = state;

            public long LockOnwer { get; } = lockOwner;

            public IEnumerable<StackLine> StackTrace { get; } = stackTrace;

            #endregion
            #region Constructors

            #endregion
        }

        #endregion
    }
}
