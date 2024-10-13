using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using TrinoClient.Serialization;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.HostAddress.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class HostAddress
    {
        #region Private Fields

        private static readonly int NO_PORT = -1;
        private static readonly Regex BRACKET_PATTERN = new("^\\[(.*:.*)\\](?::(\\d*))?$");

        #endregion

        #region Public Properties

        public string Host { get; }

        public int Port { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public HostAddress(string hostPortString)
        {
            HostAddress Address = HostAddress.FromString(hostPortString);
            Host = Address.Host;
            Port = Address.Port;
        }

        public HostAddress(string host, int port)
        {
            Host = host;
            Port = port;
        }

        #endregion

        #region Public Methods

        public static HostAddress FromString(string hostPortString)
        {
            if (string.IsNullOrEmpty(hostPortString))
            {
                throw new ArgumentNullException(nameof(hostPortString), "The host port string was null or empty.");
            }

            string Host;
            string PortString = null;

            if (hostPortString.StartsWith("["))
            {
                // Parse a bracketed host, typically an IPv6 literal.
                Match Matcher = BRACKET_PATTERN.Match(hostPortString);
                if (!Matcher.Success)
                {
                    throw new ArgumentException($"Invalid bracketed host/port: {hostPortString}");
                }

                Host = Matcher.Groups[1].Value;
                PortString = Matcher.Groups[2].Value; // could be null
            }
            else
            {
                int ColonPosition = hostPortString.IndexOf(':');

                if (ColonPosition >= 0 && hostPortString.IndexOf(':', ColonPosition + 1) == -1)
                {
                    // Exactly 1 colon.  Split into host:port.
                    Host = hostPortString.Substring(0, ColonPosition);
                    PortString = hostPortString.Substring(ColonPosition + 1);
                }
                else
                {
                    // 0 or 2+ colons.  Bare hostname or IPv6 literal.
                    Host = hostPortString;
                }
            }

            int Port = NO_PORT;

            if (!string.IsNullOrEmpty(PortString))
            {
                // Try to parse the whole port string as a number.
                if (PortString.StartsWith("+"))
                {
                    throw new ArgumentException($"Unparseable port number: {hostPortString}");
                }
                try
                {
                    Port = int.Parse(PortString);
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"Unparseable port number: {hostPortString}");
                }

                if (!IsValidPort(Port))
                {
                    throw new ArgumentException($"Port number out of range: {hostPortString}");
                }
            }

            return new HostAddress(Host, Port);
        }

        public bool HasPort()
        {
            return Port >= 0;
        }

        public override string ToString()
        {
            StringBuilder SB = new();

            if (Host.IndexOf(":") >= 0)
            {
                SB.Append("[").Append(Host).Append("]");
            }
            else
            {
                SB.Append(Host);
            }

            if (HasPort())
            {
                SB.Append(":").Append(Port);
            }

            return SB.ToString();
        }

        #endregion

        #region Private Methods

        private static bool IsValidPort(int port)
        {
            return port >= 0 && port <= 65535;
        }

        #endregion
    }
}
