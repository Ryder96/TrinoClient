using Newtonsoft.Json;

namespace TrinoClient.Model.Server
{
    /// <summary>
    /// From com.facebook.presto.server.ThreadResource.java (internal class StackLine)
    /// </summary>
    [method: JsonConstructor]    /// <summary>
                                 /// From com.facebook.presto.server.ThreadResource.java (internal class StackLine)
                                 /// </summary>
    public class StackLine(string file, int line, string className, string method)
    {
        #region Public Properties

        public string File { get; } = file;

        public int Line { get; } = line;

        public string Method { get; } = method;

        public string ClassName { get; } = className;

        #endregion
        #region Constructors

        #endregion
    }
}
