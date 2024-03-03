using Dapper;
using PowerDataNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDataCoreLib.Dapper
{
    public interface IPowerDapperHelper
    {
        int ExecuteIntegerScalar(string query, DynamicParameters parameters = null);
        Task<int> ExecuteIntegerScalarAsync(string query, DynamicParameters parameters = null);
        string ExecuteStringScalar(string query, DynamicParameters parameters = null);
        Task<string> ExecuteStringScalarAsync(string query, DynamicParameters parameters = null);
        Task<T> QueryAsync<T>(string query, DynamicParameters parameters = null);
        Task<List<T>> QueryMultipleRowsAsync<T>(string query, DynamicParameters parameters = null);
    }

    public class PowerDapperHelper : IPowerDapperHelper
    {
        private readonly ISqlConnectionStringProvider _sqlConnectionStringProvider;

        public PowerDapperHelper(ISqlConnectionStringProvider sqlConnectionStringProvider)
        {
            _sqlConnectionStringProvider = sqlConnectionStringProvider ?? throw new ArgumentNullException(nameof(sqlConnectionStringProvider));
        }

        public async Task<T> QueryAsync<T>(string query, DynamicParameters parameters = null)
        {
            using (var connection = OpenConnection())
            {
                var response = await connection.QueryAsync<T>(query, parameters, commandTimeout: 0);

                return response.FirstOrDefault();
            }
        }

        public async Task<List<T>> QueryMultipleRowsAsync<T>(string query, DynamicParameters parameters = null)
        {
            using (var connection = OpenConnection())
            {
                var response = await connection.QueryAsync<T>(query, parameters, commandTimeout: 0);

                return response.ToList();
            }
        }

        public int ExecuteIntegerScalar(string query, DynamicParameters parameters = null)
        {
            using(var connection = OpenConnection())
            {
                int response = connection.ExecuteScalar<int>(query, parameters);

                return response;
            }
        }

        public async Task<int> ExecuteIntegerScalarAsync(string query, DynamicParameters parameters = null)
        {
            using (var connection = OpenConnection())
            {
                int response = await connection.ExecuteScalarAsync<int>(query, parameters);

                return response;
            }
        }

        public string ExecuteStringScalar(string query, DynamicParameters parameters = null)
        {
            using (var connection = OpenConnection())
            {
                string response = connection.ExecuteScalar<string>(query, parameters);

                return response;
            }
        }

        public async Task<string> ExecuteStringScalarAsync(string query, DynamicParameters parameters = null)
        {
            using (var connection = OpenConnection())
            {
                string response = await connection.ExecuteScalarAsync<string>(query, parameters);

                return response;
            }
        }

        private SqlConnection OpenConnection()
        {
            var con = new SqlConnection(_sqlConnectionStringProvider.GetConnectionString());
            con.Open();

            return con;
        }
    }
}
