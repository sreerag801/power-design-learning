using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDataCoreLib.Dapper;
using PowerDataLib.Test.IntegrationTests.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PowerDataLib.Test.IntegrationTests
{
    [TestClass]
    public class PowerDapperHelperTests
    {
        IntegrationTestInitialize _context;

        [TestInitialize]
        public void TestInit()
        {
            _context = new IntegrationTestInitialize();
            _context.Init();
        }

        [TestMethod]
        public void Test_Scalar_1()
        {
            var d = _context.GetService<IPowerDapperHelper>();

            var query = @"
SELECT TOP 1 ID
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var response = d.ExecuteIntegerScalar(query);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task Test_query_1()
        {
            var d = _context.GetService<IPowerDapperHelper>();

            var query = @"
SELECT  
[ID]
,[Name]
,[Sex]
FROM [learn_sql].[dbo].[athlete_events] with(nolock)";

            var o = new DynamicParameters();
            o.Add("@Id", "1", System.Data.DbType.String, ParameterDirection.Input);
            
            var response = await d.QueryMultipleRowsAsync<AthletEvents>(query);

            Assert.IsNotNull(response);
        }
    }
}