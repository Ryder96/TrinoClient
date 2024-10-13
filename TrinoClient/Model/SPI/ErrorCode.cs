using Newtonsoft.Json;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ErrorType.java
    /// </summary>
    public class ErrorCode
    {
        #region Public Properties

        public int Code { get; }

        public string Name { get; }

        public ErrorType Type { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ErrorCode(int code, string name, ErrorType type)
        {
            ParameterCheck.OutOfRange(code >= 0, "code", "The code cannot be negative.");
            ParameterCheck.NotNullOrEmpty(name, "name");

            Code = code;
            Name = name;
            Type = type;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Name}:{Code}";
        }

        #endregion
    }
}
