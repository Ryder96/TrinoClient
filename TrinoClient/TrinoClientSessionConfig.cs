using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TrinoClient.Model;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient
{
    /// <summary>
    /// The set of options that can be supplied to the TrinoClient to configure
    /// how the client connects to the coordinator and what catalog and schema to use.
    /// 
    /// There are several options such User and Properties that will apply to all
    /// queries made using the config passed to the TrinoClient. Items like Properties
    /// can also be specified in QueryOptions, which will add their values to the values
    /// provided here, not overwrite them.
    /// </summary>
    public sealed class TrinoClientSessionConfig
    {
        #region Defaults
        private static readonly string _DEFAULT_HOST = "localhost";
        private static readonly int _DEFAULT_PORT = 8080;
        private static readonly long _DEFAULT_TIMEOUT = -1; // Anything 0 or below indicates that the client will never timeout a query

        #endregion

        #region Private Fields

        private string _User;
        private int _Port;
        private int _CheckInterval;
        private HashSet<string> _ClientTags;
        private IDictionary<string, string> _Properties;

        #endregion

        #region Public Properties

        /// <summary>
        /// The DNS host name or IP address of the presto coordinator.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The web interface port of the presto coordinator, usually 8080.
        /// </summary>
        public int Port
        {
            get
            {
                return _Port;
            }
            set
            {
                if (value <= 65535 && value >= 1)
                {
                    _Port = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Port", "The port must be between 1 and 65535.");
                }
            }
        }

        /// <summary>
        /// The name of the user connecting to presto. This defaults to the current
        /// user.
        /// </summary>
        public string User
        {
            get
            {
                return _User;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("User", "The user name cannot be null or empty.");
                }

                _User = value;
            }
        }

        /// <summary>
        /// The password to use in HTTP basic auth with the presto server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The catalog to use for interaction with presto.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// The schema to connect to in presto. This defaults to 'default'.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Whether to ignore SSL errors produced by connecting to presto over an SSL
        /// connection that may be using expired or untrusted certificates.
        /// </summary>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Specifies that the connections made to the presto coordinator and possibly
        /// worker nodes should use SSL. 
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// The version of the REST API being used, this defaults to V1.
        /// </summary>
        public TrinoApiVersion Version { get; }

        /// <summary>
        /// Extra info about the client making the query
        /// </summary>
        public string ClientInfo { get; set; }

        /// <summary>
        /// This will always default to "0" representing UTC
        /// </summary>
        public TimeZoneKey TimeZone { get; }

        /// <summary>
        /// Defaults to CurrentCulture
        /// </summary>
        public CultureInfo Locale { get; set; }

        /// <summary>
        /// Properties to add to the session that are used by the Presto 
        /// engine or connectors to customize the query execution, for example:
        /// 
        /// { "hive.optimized_reader_enabled", "true" }
        /// 
        /// or
        /// 
        /// { "optimize_hash_generation", "true" }
        /// 
        /// See https://prestodb.io/docs/current/admin/properties.html for information on
        /// available properties that can be set
        /// </summary>
        public IDictionary<string, string> Properties
        {

            get
            {
                return _Properties;
            }
            set
            {
                foreach (KeyValuePair<string, string> Item in value)
                {
                    if (string.IsNullOrEmpty(Item.Key))
                    {
                        throw new ArgumentNullException("Properties", "Session property key name is empty.");
                    }

                    if (Item.Key.Contains("="))
                    {
                        throw new FormatException($"Session property name must not contain '=' : {Item.Key}");
                    }

                    string AsciiKey = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Key));

                    if (!AsciiKey.Equals(Item.Key))
                    {
                        throw new FormatException($"Session property name contains non US_ASCII characters: {Item.Key}");
                    }

                    string AsciiValue = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Value));

                    if (!AsciiValue.Equals(Item.Value))
                    {
                        throw new FormatException($"Session property value contains non US_ASCII characters: {Item.Value}");
                    }
                }

                _Properties = value;
            }
        }

        /// <summary>
        /// A set of prepared statements. The key value is the statement
        /// name and the value is the statement, for example:
        /// 
        /// { "my_select_1", "SELECT * FROM nation WHERE regionKey = ? and nationKey > ?"}
        /// 
        /// This could then be executed with EXECUTE my_select_1 USING 1, 3;
        /// 
        /// 1 is provided the '?' to compare against regionKey and 3 is provided to '?' to compare
        /// against nationKey.
        /// </summary>
        public IDictionary<string, string> PreparedStatements { get; set; }

        /// <summary>
        /// Client provided tags that are associated with the session
        /// and are displayed in the SessionRepresentation
        /// </summary>
        public HashSet<string> ClientTags
        {

            get
            {
                return _ClientTags;
            }
            set
            {
                foreach (string Tag in value)
                {
                    if (Tag.Contains(","))
                    {
                        throw new ArgumentException($"The client tag {Tag} cannot contain a comma ','.");
                    }
                }

                _ClientTags = value;
            }
        }

        /// <summary>
        /// Enables debug information
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// The timeout in seconds for how long to wait for a query to finish
        /// </summary>
        public long ClientRequestTimeout { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new client options set with default values.
        /// </summary>
        public TrinoClientSessionConfig()
        {
            Host = _DEFAULT_HOST;
            Port = _DEFAULT_PORT;
            User = Environment.GetEnvironmentVariable("USERNAME") ?? Environment.GetEnvironmentVariable("USER");
            IgnoreSslErrors = false;
            UseSsl = false;
            Version = TrinoApiVersion.V1;
            ClientTags = [];
            Debug = false;
            Properties = new Dictionary<string, string>();
            PreparedStatements = new Dictionary<string, string>();
            TimeZone = TimeZoneKey.GetTimeZoneKey(0);
            Locale = CultureInfo.CurrentCulture;
            ClientRequestTimeout = _DEFAULT_TIMEOUT;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        public TrinoClientSessionConfig(string host, int port, string catalog) : this()
        {
            Host = host;
            Port = port;
            Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        public TrinoClientSessionConfig(string catalog) : this()
        {
            Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public TrinoClientSessionConfig(string catalog, string schema) : this(catalog)
        {
            Schema = schema;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public TrinoClientSessionConfig(string host, string catalog, string schema) : this(catalog, schema)
        {
            Host = host;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public TrinoClientSessionConfig(string host, int port, string catalog, string schema) : this(host, port, catalog)
        {
            Schema = schema;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        /// <param name="user">The user name associated with this session</param>
        /// <param name="clientTags">Client tags to apply to all requests in this session</param>
        /// <param name="clientInfo">Additional info about the client</param>
        /// <param name="locale">The non-default locale to use for language settings</param>
        /// <param name="properties">Persistent session properties to set</param>
        /// <param name="preparedStatements">Prepared statements that will be available to all queries made in this session</param>
        /// <param name="debug">Whether to enable debug logging for this session</param>
        /// <param name="timeout">The timeout in seconds for queries in this session</param>
        public TrinoClientSessionConfig(
            string host,
            int port,
            string catalog,
            string schema,
            string user,
            HashSet<string> clientTags,
            string clientInfo,
            CultureInfo locale,
            IDictionary<string, string> properties,
            IDictionary<string, string> preparedStatements,
            bool debug,
            long timeout
            ) : this(host, port, catalog, schema)
        {
            User = user;
            ClientTags = clientTags;
            ClientInfo = clientInfo;
            Locale = locale;
            Properties = properties;
            PreparedStatements = preparedStatements;
            Debug = debug;
            ClientRequestTimeout = timeout;
        }

        #endregion
    }
}
