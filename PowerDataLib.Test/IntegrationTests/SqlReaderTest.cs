using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDataCoreLib.SqlServer;
using PowerDataLib.Test.IntegrationTests.Models;
using PowerDataNet;
using PowerDataNet.SqlServer;
using System.Data.SqlClient;

namespace PowerDataLib.Test.IntegrationTests
{
    [TestClass]
    public class SqlReaderTest
    {
        IntegrationTestInitialize _context;

        [TestInitialize]
        public void TestInit()
        {
            _context = new IntegrationTestInitialize();
            _context.Init();
        }

        [TestMethod]
        [Ignore]
        public void Test_1()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"
SELECT TOP (10) [ID]
,[Name]
,[Sex]   
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var r = sqlReader.SqlExecuteReader(query, (r) => Map(r));

            Assert.IsNotNull(r);
        }

        [TestMethod]
        [Ignore]
        public void Test_2()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"
SELECT TOP (1) [ID]
,[Name]
,[Sex]   
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var r = sqlReader.SqlExecuteObjectReader(query, (r) => Map(r));

            Assert.IsNotNull(r);
        }

        [TestMethod]
        [Ignore]
        public void Test_3()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"
SELECT TOP (1) [ID]
,[Name]
,[Sex]   
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var r = sqlReader.SqlExecuteScalarIntegerOrNull(query);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        [Ignore]
        public void Test_4()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"UPDATE [learn_sql].[dbo].[athlete_events] SET Name = @Name WHERE ID = @Id;";

            var r = sqlReader.SqlExecuteNonQuery(query, (c) =>
            {
                c.Parameters.AddWithValue("@Name", "coder-sree");
                c.Parameters.AddWithValue("@Id", "1");
            });

            Assert.IsTrue(r == 1);
        }

        [TestMethod]
        [Ignore]
        public void Test_5()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"
SELECT TOP (1) [ID]
,[Name]
,[Sex]   
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var r = sqlReader.SqlExecuteScalarStringOrNull(query);

            Assert.IsTrue(r == "1");
        }

        [TestMethod]
        //[Ignore]
        public void Test_6()
        {
            var service = _context.GetService<ISqlConnectionStringProvider>();

            var sqlReader = _context.GetService<ISqlReader>();

            var query = @"
SELECT TOP (1) [ID]
,[Name]
,[Sex]   
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var r = sqlReader.SqlExecuteReader(query, (reader) => {
                 return reader.ReadStringOrNull("Name");
            });

            Assert.IsTrue(r == "1");
        }

        [TestMethod]
        //[Ignore]
        public void Test_7()
        {
            AthletEvents a = new AthletEvents()
            {
                Age = 10
            };

            AthletEvents b = a;

            a.Age = 20;

            Assert.IsTrue(b.Age == 20);
        }
        public AthletEvents Map(SqlDataReader r)
        {
            return new AthletEvents()
            {
                ID = r["ID"].ToString(),
                Name = r["Name"].ToString(),
                Sex = r["Sex"].ToString()
            };
        }
    }
}
