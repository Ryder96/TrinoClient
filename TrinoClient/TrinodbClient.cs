using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrinoClient.Interfaces;
using TrinoClient.Model;
using TrinoClient.Model.Jmx;
using TrinoClient.Model.NodeInfo;
using TrinoClient.Model.Query;
using TrinoClient.Model.SPI;
using TrinoClient.Model.Statement;
using TrinoClient.Model.Thread;

namespace TrinoClient
{
    /// <summary>
    /// The client used to connect to a presto coordinator and execute statements, query nodes, get query info and summaries, and terminate running queries.
    /// </summary>
    public class TrinodbClient : ITrinoClient
    {
        #region Private Properties

        private static readonly string AssemblyVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// The cookie container all of the handlers will use so they all have access to the same cookies
        /// </summary>
        private CookieContainer Cookies = new();

        /// <summary>
        /// A normal http handler
        /// </summary>
        private HttpClientHandler NormalHandler;

        /// <summary>
        /// A handler that provides a log when SSL errors occur and then ignores those errors
        /// </summary>
        private HttpClientHandler IgnoreSslErrorHandler;

        /// <summary>
        /// The standard HTTP client using the Handler
        /// </summary>
        private HttpClient NormalClient;

        /// <summary>
        /// An HTTP client that ignores SSL errors
        /// </summary>
        private HttpClient IgnoreSslErrorClient;

        /// <summary>
        /// Initializes HTTP Handler and Client
        /// </summary>
        private void InitializeHttpClients()
        {
            NormalHandler = new HttpClientHandler()
            {
                CookieContainer = Cookies,
                ServerCertificateCustomValidationCallback = (request, cert, chain, sslPolicyErrors) =>
                {
                    return sslPolicyErrors == SslPolicyErrors.None;
                }
            };

            IgnoreSslErrorHandler = new HttpClientHandler()
            {
                CookieContainer = Cookies,
                ServerCertificateCustomValidationCallback = (request, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };

            NormalClient = new HttpClient(NormalHandler);
            IgnoreSslErrorClient = new HttpClient(IgnoreSslErrorHandler);
        }

        #endregion

        #region Public Properties

        public TrinoClientSessionConfig Configuration { get; private set; }

        #endregion

        #region Constructor

        public TrinodbClient()
        {
            // Initialize with defaults
            Configuration = new TrinoClientSessionConfig();

            InitializeHttpClients();
        }

        public TrinodbClient(TrinoClientSessionConfig config)
        {
            Configuration = config ?? throw new ArgumentNullException(nameof(config), "The presto client configuration cannot be null.");

            InitializeHttpClients();
        }

        #endregion

        #region Public Methods

        #region Threads

        /// <summary>
        /// Gets information about the in use threads in the cluster.
        /// </summary>
        /// <returns>
        /// Information about all of the threads and their state.
        /// </returns>
        public async Task<ListThreadsV1Response> ListThreads()
        {
            return await ListThreads(CancellationToken.None);
        }

        /// <summary>
        /// Gets information about the in use threads in the cluster.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about all of the threads and their state.
        /// </returns>
        public async Task<ListThreadsV1Response> ListThreads(CancellationToken cancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            Uri Path = BuildUri("/thread");

            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request, cancellationToken);

            string Json = await Response.Content.ReadAsStringAsync(cancellationToken);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                ListThreadsV1Response Result = new(Json);

                return Result;
            }
        }

        /// <summary>
        /// Gets the web ui html that displays information about the threads
        /// in the cluster and optionally opens that web page.
        /// </summary>
        /// <returns>The web page html/javascript/css.</returns>
        public async Task<string> GetThreadUIHtml()
        {
            return await GetThreadUIHtml(CancellationToken.None);
        }

        /// <summary>
        /// Gets the web ui html that displays information about the threads
        /// in the cluster and optionally opens that web page.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The web page html/javascript/css.</returns>
        public async Task<string> GetThreadUIHtml(CancellationToken cancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            StringBuilder SB = new();

            string Scheme = Configuration.UseSsl ? "https" : "http";
            SB.Append($"{Scheme}://{Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && Configuration.Port != 80) || (Scheme == "https" && Configuration.Port != 443))
            {
                SB.Append($":{Configuration.Port}");
            }

            SB.Append($"/ui/thread");

            Uri Path = new(SB.ToString());

            HttpResponseMessage Response = await LocalClient.GetAsync(Path, cancellationToken);

            string Html = await Response.Content.ReadAsStringAsync(cancellationToken);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                return Html;
            }
        }

        #endregion

        #region Nodes

        /// <summary>
        /// Gets the worker nodes in a presto cluster
        /// </summary>
        /// <returns>
        /// Information about all of the worker nodes. If the request is unsuccessful, 
        /// a PrestoException is thrown.
        /// </returns>
        public async Task<ListNodesV1Response> ListNodes()
        {
            return await ListNodes(CancellationToken.None);
        }

        /// <summary>
        /// Gets the worker nodes in a presto cluster
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about all of the worker nodes. If the request is unsuccessful, 
        /// a PrestoException is thrown.
        /// </returns>
        public async Task<ListNodesV1Response> ListNodes(CancellationToken cancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            Uri Path = BuildUri("/node");

            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request, cancellationToken);

            string Json = await Response.Content.ReadAsStringAsync(cancellationToken);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                ListNodesV1Response Result = new(Json);

                return Result;
            }
        }

        /// <summary>
        /// Gets any failed nodes in a presto cluster.
        /// </summary>
        /// <returns>
        /// Information about the failed nodes. If there are no failed nodes, 
        /// the FailedNodes property will be null.
        /// </returns>
        public async Task<ListFailedNodesV1Response> ListFailedNodes()
        {
            return await ListFailedNodes(CancellationToken.None);
        }

        /// <summary>
        /// Gets any failed nodes in a presto cluster.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// Information about the failed nodes. If there are no failed nodes, 
        /// the FailedNodes property will be null.
        /// </returns>
        public async Task<ListFailedNodesV1Response> ListFailedNodes(CancellationToken cancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            StringBuilder SB = new();

            string Scheme = Configuration.UseSsl ? "https" : "http";
            SB.Append($"{Scheme}://{Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && Configuration.Port != 80) || (Scheme == "https" && Configuration.Port != 443))
            {
                SB.Append($":{Configuration.Port}");
            }

            SB.Append($"/{GetVersionString(Configuration.Version)}/node/failed");

            Uri Path = new(SB.ToString());

            HttpResponseMessage Response = await LocalClient.GetAsync(Path, cancellationToken);

            string Json = await Response.Content.ReadAsStringAsync(cancellationToken);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                ListFailedNodesV1Response Result = new(Json);

                return Result;
            }
        }

        #endregion

        #region Query

        /// <summary>
        /// Kills an active query statement
        /// </summary>
        /// <param name="queryId">The id of the query to kill</param>
        /// <returns>No value is returned, but the method will throw an exception if it was not successful</returns>
        public async Task KillQuery(string queryId)
        {
            await KillQuery(queryId, CancellationToken.None);
        }

        /// <summary>
        /// Kills an active query statement
        /// </summary>
        /// <param name="queryId">The Id of the query to kill</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>No value is returned, but the method will throw an exception if it was not successful</returns>
        public async Task KillQuery(string queryId, CancellationToken cancellationToken)
        {
            HttpClient localClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            Uri Path = BuildUri($"/query/{queryId}");

            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Delete);

            HttpResponseMessage Response = await localClient.SendAsync(Request, cancellationToken);

            // Expect a 204 response
            if (Response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
        }

        /// <summary>
        /// This method returns information and statistics about queries that
        /// are currently being executed on a Presto coordinator that have not been purged
        /// </summary>
        /// <returns>Details on the queries</returns>
        public async Task<ListQueriesV1Response> GetQueries()
        {
            return await GetQueries(CancellationToken.None);
        }

        /// <summary>
        /// This method returns information and statistics about queries that
        /// are currently being executed on a Presto coordinator that have not been purged
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Details on the queries</returns>
        public async Task<ListQueriesV1Response> GetQueries(CancellationToken cancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            Uri Path = BuildUri($"/query");

            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.SendAsync(Request, cancellationToken);

            // Expect a 200 response
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                ListQueriesV1Response Results = new(await Response.Content.ReadAsStringAsync(cancellationToken));
                return Results;
            }
        }

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Detailed summary of the query</returns>
        public async Task<GetQueryV1Response> GetQuery(string queryId)
        {
            return await GetQuery(queryId, CancellationToken.None);
        }

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Detailed summary of the query</returns>
        public async Task<GetQueryV1Response> GetQuery(string queryId, CancellationToken CancellationToken)
        {
            HttpClient LocalClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            Uri Path = BuildUri($"/query/{queryId}");

            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Get);

            HttpResponseMessage Response = await LocalClient.GetAsync(Path, CancellationToken);

            // Expect a 200 response
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(CancellationToken), Response.StatusCode);
            }
            else
            {
                GetQueryV1Response Result = new(await Response.Content.ReadAsStringAsync(CancellationToken));
                return Result;
            }
        }

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <returns>Detailed summary of the query</returns>
        public async Task<GetQueryV1Response> GetQuery(QueryId queryId)
        {
            return await GetQuery(queryId, CancellationToken.None);
        }

        /// <summary>
        /// Gets a detailed summary of the specified query
        /// </summary>
        /// <param name="queryId">The id of the query to retrieve details about.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Detailed summary of the query</returns>
        public async Task<GetQueryV1Response> GetQuery(QueryId queryId, CancellationToken cancellationToken)
        {
            return await GetQuery(queryId.ToString(), cancellationToken);
        }

        #endregion

        #region Statements

        /// <summary>
        /// Submits a Presto SQL statement for execution. The Presto client 
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request.</param>
        /// <returns>The resulting response object from the query.</returns>
        public virtual async Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request)
        {
            return await ExecuteQueryV1(request, CancellationToken.None);
        }

        /// <summary>
        /// Submits a Presto SQL statement for execution. The Presto client 
        /// executes queries on behalf of a user against a catalog and a schema.
        /// </summary>
        /// <param name="request">The query execution request.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The resulting response object from the query.</returns>
        public async Task<ExecuteQueryV1Response> ExecuteQueryV1(ExecuteQueryV1Request request, CancellationToken cancellationToken)
        {
            // Check the required configuration items before running the query
            if (!CheckConfiguration(out Exception ex))
            {
            throw ex;
            }

            // Track all of the incremental results as they are returned
            var results = new List<QueryResultsV1>();

            // Choose the correct client to use for SSL errors
            var localClient = Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient;

            // Build the URL path
            var path = BuildUri("/statement");

            // Create a new request to post with the query
            var httpRequest = BuildRequest(path, HttpMethod.Post, new StringContent(request.Query));

            // Add all of the configured headers to the request
            BuildQueryHeaders(ref httpRequest, request.Options);

            // Use the stopwatch to measure if we've exceeded the specified timeout
            var stopwatch = new Stopwatch();
            if (Configuration.ClientRequestTimeout > 0)
            {
            stopwatch.Start();
            }

            // This is the original submission result, will contain the nextUri property to follow in order to get the results
            var responseMessage = await MakeHttpRequest(localClient, httpRequest, cancellationToken: cancellationToken);

            // This doesn't really do anything but evaluate the headers right now
            ProcessResponseHeaders(responseMessage);

            var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

            // If parsing the submission response fails, return and exit
            if (!QueryResultsV1.TryParse(content, out var response, out var parseEx))
            {
            throw new TrinoException("The query submission response could not be parsed.", content, parseEx);
            }

            // Check to make sure there wasn't an error provided
            if (response.Error != null)
            {
            throw new TrinoQueryException(response.Error);
            }

            results.Add(response);

            // Keep track of the last, non-null URI so we can send a delete request to it at the end
            var lastUri = path;

            // Recursively fetch results from the next URI
            await FetchNextResults(localClient, response.NextUri, results, cancellationToken, stopwatch);

            // Explicitly closes the query
            responseMessage = await localClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, lastUri), cancellationToken);

            // If a 204 is not returned, the query was not successfully closed
            var closed = responseMessage.StatusCode == HttpStatusCode.NoContent;

            return new ExecuteQueryV1Response(results, closed);
        }
        private async Task FetchNextResults(HttpClient localClient, Uri nextUri, List<QueryResultsV1> results, CancellationToken cancellationToken, Stopwatch stopwatch)
        {
            if (nextUri == null || (Configuration.ClientRequestTimeout > 0 && stopwatch.Elapsed.TotalSeconds > Configuration.ClientRequestTimeout))
            {
            return;
            }

            // Make the request and get back a valid response, otherwise
            // the MakeRequest method will throw an exception
            var request = BuildRequest(nextUri, HttpMethod.Get);

            var responseMessage = await MakeHttpRequest(localClient, request, cancellationToken: cancellationToken);

            ProcessResponseHeaders(responseMessage);

            var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

            // Make sure deserialization succeeded
            if (QueryResultsV1.TryParse(content, out var response, out var parseEx))
            {
            results.Add(response);

            // Check to make sure there wasn't an error provided
            if (response.Error != null)
            {
                throw new TrinoQueryException(response.Error);
            }

            // Recursively fetch results from the next URI
            await FetchNextResults(localClient, response.NextUri, results, cancellationToken, stopwatch);
            }
            else
            {
            throw new TrinoException("The response from presto could not be deserialized.", content, parseEx);
            }
        }

        /// <summary>
        /// Gets details about a specified Jmx Mbean
        /// </summary>
        /// <param name="request">The request details</param>
        /// <returns>Details about the specified Jmx Mbean</returns>
        public async Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request)
        {
            return await GetJmxMbean(request, CancellationToken.None);
        }

        /// <summary>
        /// Gets details about a specified Jmx Mbean
        /// </summary>
        /// <param name="request">The request details</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>Details about the specified Jmx Mbean</returns>
        public async Task<JmxMbeanV1Response> GetJmxMbean(JmxMbeanV1Request request, CancellationToken cancellationToken)
        {
            // Check the required configuration items before running the query
            if (!CheckConfiguration(out Exception Ex))
            {
                throw Ex;
            }

            // Build the url path
            Uri Path = BuildUri($"/jmx/mbean/{request.ObjectName}");

            // Create a new request to post with the query
            HttpRequestMessage Request = BuildRequest(Path, HttpMethod.Get);

            // Submit the request for details on the requested object name
            HttpResponseMessage Response = await MakeHttpRequest(Request, cancellationToken);

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw new TrinoWebException(await Response.Content.ReadAsStringAsync(cancellationToken), Response.StatusCode);
            }
            else
            {
                // Generate a new query response object
                return new JmxMbeanV1Response(await Response.Content.ReadAsStringAsync(cancellationToken));
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes a request with the provided request message, choosing the appropriate http client from the
        /// configuration parameters and returns the response message
        /// </summary>
        /// <param name="client">The http client to use to make the request.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <param name="maxRetries">The maximum number of times the method will try to contact the server
        /// if a service unavailable response code is returned.</param>
        /// <returns>The http response message from the request.</returns>
        private async Task<HttpResponseMessage> MakeHttpRequest(HttpRequestMessage request, CancellationToken cancellationToken, uint maxRetries = 5)
        {
            return await MakeHttpRequest(Configuration.IgnoreSslErrors ? IgnoreSslErrorClient : NormalClient, request, cancellationToken, maxRetries);
        }


        /// <summary>
        /// Makes a request with the provided client and request message and
        /// returns the response message
        /// </summary>
        /// <param name="client">The http client to use to make the request.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <param name="maxRetries">The maximum number of times the method will try to contact the server
        /// if a service unavailable response code is returned.</param>
        /// <returns>The http response message from the request.</returns>
        private static async Task<HttpResponseMessage> MakeHttpRequest(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken, uint maxRetries = 5)
        {
            HttpResponseMessage response = null;
            uint Counter = 0;

            while (Counter < maxRetries)
            {
                response = await client.SendAsync(request, cancellationToken);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            return response;
                        }
                    case HttpStatusCode.ServiceUnavailable:
                        {
                            // Retry with an exponential backoff
                            int Milliseconds = (int)Math.Floor(Math.Pow(2, Counter) * 1000);
                            Milliseconds += new Random().Next(0, 1000);
                            await Task.Delay(Milliseconds, cancellationToken);

                            Counter++;

                            break;
                        }
                    default:
                        {
                            throw new TrinoWebException($"The request to {request.RequestUri} failed, message:{await response.Content.ReadAsStringAsync()}", response.StatusCode);
                        }
                }
            }

            // This will only be reached if the while loop exits
            throw new TrinoWebException($"The maximum number of retries, {maxRetries}, for path {request.RequestUri} was exceeded.", response.StatusCode);
        }

        /// <summary>
        /// Builds the URI
        /// </summary>
        /// <param name="relativePath">The relative path to append to the base path, not including the version</param>
        /// <param name="version">The version of the API to use, will precede the relative path</param>
        /// <returns></returns>
        private Uri BuildUri(string relativePath, TrinoApiVersion version)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath), "The relative path in the url being constructed cannot be null or empty.");
            }

            StringBuilder SB = new();

            string Scheme = Configuration.UseSsl ? "https" : "http";
            SB.Append($"{Scheme}://{Configuration.Host}");

            // Only add non-standard ports
            if ((Scheme == "http" && Configuration.Port != 80) || (Scheme == "https" && Configuration.Port != 443))
            {
                SB.Append($":{Configuration.Port}");
            }

            if (!relativePath.StartsWith('/'))
            {
                relativePath = $"/{relativePath}";
            }

            SB.Append($"/{GetVersionString(version)}{relativePath}");

            Uri Path = new(SB.ToString());

            return Path;
        }

        /// <summary>
        /// Builds the URI. The version in the path default to the client configuration
        /// </summary>
        /// <param name="relativePath">The relative path to append to the base path, not including the version.</param>
        /// <returns></returns>
        private Uri BuildUri(string relativePath)
        {
            return BuildUri(relativePath, Configuration.Version);
        }

        /// <summary>
        /// Checks the current configuration 
        /// </summary>
        /// <param name="ex">The exception to throw if a parameter is not configured correctly</param>
        /// <returns>True if the configuration is consistent, false if not</returns>
        private bool CheckConfiguration(out Exception ex)
        {
            if (string.IsNullOrEmpty(Configuration.User))
            {
                ex = new ArgumentNullException("user", "The user was not specified.");
                return false;
            }

            if (string.IsNullOrEmpty(Configuration.Catalog) && !string.IsNullOrEmpty(Configuration.Schema))
            {
                ex = new ArgumentException("The Schema cannot be set without setting the catalog.");
                return false;
            }

            ex = null;
            return true;
        }

        /// <summary>
        /// Adds the session level and query specific options as headers to the HttpRequestMessage
        /// </summary>
        /// <param name="request">The HttpRequestMessage that will have headers added to it</param>
        /// <param name="options">The set of query specific options</param>
        private void BuildQueryHeaders(ref HttpRequestMessage request, QueryOptions options = null)
        {
            // Defaults that will remain static
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Timezone will always be set
            request.Headers.Add(TrinoHeader.TRINO_TIME_ZONE.Value, Configuration.TimeZone.Id);

            // Catalog
            if (!string.IsNullOrEmpty(Configuration.Catalog))
            {
                request.Headers.Add(TrinoHeader.TRINO_CATALOG.Value, Configuration.Catalog);
            }

            // Schema
            if (!string.IsNullOrEmpty(Configuration.Schema))
            {
                request.Headers.Add(TrinoHeader.TRINO_SCHEMA.Value, Configuration.Schema);
            }

            // ClientInfo
            if (!string.IsNullOrEmpty(Configuration.ClientInfo))
            {
                request.Headers.Add(TrinoHeader.TRINO_CLIENT_INFO.Value, Configuration.ClientInfo);
            }

            // Language
            if (Configuration.Locale != null)
            {
                request.Headers.Add(TrinoHeader.TRINO_LANGUAGE.Value, Configuration.Locale.Name);
            }

            // Session properties
            if (Configuration.Properties != null)
            {
                foreach (KeyValuePair<string, string> Property in Configuration.Properties)
                {
                    request.Headers.Add(TrinoHeader.TRINO_SESSION.Value, $"{Property.Key}={Property.Value}");
                }
            }

            // Build final set of prepared statements
            IDictionary<string, string> PreparedStatements = new Dictionary<string, string>();

            if (Configuration.PreparedStatements != null)
            {
                foreach (KeyValuePair<string, string> Item in Configuration.PreparedStatements)
                {
                    PreparedStatements.Add(Item);
                }
            }

            // Build final set of tags
            HashSet<string> Tags = [];

            if (Configuration.ClientTags != null)
            {
                // Client tags are not allowed to have commas in them and have already been checked in the setter
                foreach (string Tag in Configuration.ClientTags)
                {
                    Tags.Add(Tag);
                }
            }

            if (options != null)
            {
                // Add query specific client tags
                if (options.ClientTags != null)
                {
                    foreach (string Tag in options.ClientTags)
                    {
                        Tags.Add(Tag);
                    }
                }

                // Add query specific prepared statements
                if (options.PreparedStatements != null)
                {
                    foreach (KeyValuePair<string, string> Statement in options.PreparedStatements)
                    {
                        if (!PreparedStatements.ContainsKey(Statement.Key))
                        {
                            PreparedStatements.Add(Statement);
                        }
                    }
                }

                // Add query specific session properties
                if (options.Properties != null)
                {
                    foreach (KeyValuePair<string, string> Property in options.Properties)
                    {
                        request.Headers.Add(TrinoHeader.TRINO_SESSION.Value, $"{Property.Key}={Property.Value}");
                    }
                }

                if (!string.IsNullOrEmpty(options.TransactionId))
                {
                    request.Headers.Add(TrinoHeader.TRINO_TRANSACTION_ID.Value, options.TransactionId);
                }
                else
                {
                    request.Headers.Add(TrinoHeader.TRINO_TRANSACTION_ID.Value, "NONE");
                }
            }

            // If any session or query prepared statements were provided, add them
            if (PreparedStatements.Any())
            {
                foreach (KeyValuePair<string, string> Statement in PreparedStatements)
                {
                    request.Headers.Add(TrinoHeader.TRINO_PREPARED_STATEMENT.Value, $"{WebUtility.UrlDecode(Statement.Key)}={WebUtility.UrlDecode(Statement.Value)}");
                }
            }

            // If any session or query client tags were provided, add tem
            if (Tags.Count != 0)
            {
                request.Headers.Add(TrinoHeader.TRINO_CLIENT_TAGS.Value, string.Join(",", Tags));
            }
        }

        /// <summary>
        /// Gets the string representation of the PrestoApiVersion
        /// </summary>
        /// <param name="version">The presto api version to get the string value of</param>
        /// <returns>The api version string</returns>
        private static string GetVersionString(TrinoApiVersion version)
        {
            return version.GetType().GetMember(version.ToString()).FirstOrDefault().GetCustomAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// Builds a new HttpRequestMessage with basic common headers,
        /// User-Agent, Accept, X-Presto-Source, and X-Presto-User
        /// </summary>
        /// <param name="url">The url for the request</param>
        /// <param name="method">The Http method</param>
        /// <param name="content">Any content included with the request</param>
        /// <returns></returns>
        private HttpRequestMessage BuildRequest(Uri url, HttpMethod method, HttpContent content = null)
        {
            HttpRequestMessage request = new(method, url);

            if (content != null)
            {
                request.Content = content;
            }

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", $"trinodb_dotnet_core_sdk/{AssemblyVersion}");
            request.Headers.Add(TrinoHeader.TRINO_SOURCE.Value, "trinodb_dotnet_core_sdk");

            if (!string.IsNullOrEmpty(Configuration.User))
            {
                request.Headers.Add(TrinoHeader.TRINO_USER.Value, Configuration.User.Replace(":", ""));

                if (!string.IsNullOrEmpty(Configuration.Password))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Configuration.User.Replace(":", "")}:{Configuration.Password}")));
                }
            }

            return request;
        }

        /// <summary>
        /// Processes the http response headers for values supplied from presto
        /// </summary>
        /// <param name="response">The http response message from presto</param>
        /// <returns>The collection of values that were set by presto in the http headers</returns>
        private static ResponseHeaderCollection ProcessResponseHeaders(HttpResponseMessage response)
        {
            return new ResponseHeaderCollection(response.Headers);
        }

        #endregion
    }
}
