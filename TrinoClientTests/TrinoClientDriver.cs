using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrinoClient.Interfaces;
using TrinoClient.Model.Query;
using TrinoClient.Model.Server;
using TrinoClient.Model.Statement;
using Xunit;

namespace TrinoClient.Tests
{
    public class TrinoClientDriver
    {
        private static string Schema = "cars";
        private static string S3_Location = "";

        public TrinoClientDriver()
        { }

        [Fact]
        public async Task TestPassword()
        {
            // ARRANGE
            TrinoClientSessionConfig config = new()
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"
            };

            ITrinoClient client = new TrinodbClient(config);

            // ACT
            ExecuteQueryV1Request req = new($"CREATE SCHEMA IF NOT EXISTS hive.{Schema}");

            //mock.Verify(x => x.ExecuteQueryV1(captor.Capture()));
            //req.

            ExecuteQueryV1Response res = await client.ExecuteQueryV1(req);

            // ASSERT
            Assert.NotNull(res);
        }

        [Fact]
        public async Task CreateSchema()
        {
            // ARRANGE
            TrinoClientSessionConfig Config = new()
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };

            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new($"CREATE SCHEMA IF NOT EXISTS hive.{Schema}");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task CreateTable()
        {
            // ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };

            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new($"CREATE TABLE IF NOT EXISTS tracklets (id bigint, objectclass varchar, length double, trackdata array(varchar), platform varchar,spectrum varchar, timestamp bigint) WITH (format = 'AVRO', external_location = '{S3_Location}');");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatement()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementOrderBy()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new("select * from tracklets ORDER BY length limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestExecuteStatementWhere()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new("select id,length,objectclass from tracklets WHERE length > 1000 LIMIT 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            // ASSERT
            Assert.True(Res.QueryClosed == true);
        }

        [Fact]
        public async Task TestQueryResultDataToJson()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            string Json = Res.DataToJson();

            // ASSERT
            Assert.True(Res.QueryClosed == true && !string.IsNullOrEmpty(Json));
        }

        [Fact]
        public async Task TestQueryResultDataToCsv()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            ExecuteQueryV1Request Req = new("select * from tracklets limit 5;");

            // ACT
            ExecuteQueryV1Response Res = await Client.ExecuteQueryV1(Req);

            string Csv = string.Join("\n", Res.DataToCsv());

            // ASSERT
            Assert.True(Res.QueryClosed == true && !string.IsNullOrEmpty(Csv));
        }

        [Fact]
        public async Task TestListQueries()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            // ACT
            ListQueriesV1Response Res = await Client.GetQueries();

            // ASSERT
            Assert.True(Res != null && Res.DeserializationSucceeded);
        }

        [Fact]
        public async Task TestGetQuery()
        {
            //ARRANGE
            TrinoClientSessionConfig Config = new("hive", Schema)
            {
                Host = "172.16.0.232",
                Port = 8080,
                User = "admin"

            };
            ITrinoClient Client = new TrinodbClient(Config);

            // ACT
            ListQueriesV1Response Res = await Client.GetQueries();

            List<GetQueryV1Response> Info = [];

            foreach (BasicQueryInfo Item in Res.QueryInfo)
            {
                GetQueryV1Response QRes = await Client.GetQuery(Item.QueryId);
                Info.Add(QRes);
            }

            // ASSERT
            Assert.True(Res != null && Info.All(x => x.DeserializationSucceeded == true));
        }
    }
}
