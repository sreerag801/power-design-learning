using Microsoft.Extensions.Configuration;
using System;

namespace PowerDataNet
{
    public interface ISqlConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class GetSqlConnectionFromConfiguration : ISqlConnectionStringProvider
    {
        string _connectionString;
        IConfiguration _configuration;

        public GetSqlConnectionFromConfiguration(IConfiguration configuration,
            string connectionName)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            string connectionString = _configuration.GetConnectionString(connectionName);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception($"Connection string value for {connectionName} is null or empty.");

            _connectionString = connectionString;
        }
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}