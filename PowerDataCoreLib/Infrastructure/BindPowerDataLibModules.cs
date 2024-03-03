using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerDataCoreLib.Dapper;
using PowerDataNet.SqlServer;

namespace PowerDataNet.Infrastructure
{
    public static class BindPowerDataLibModules
    {
        public static void BindPowerDataLibConnectionProvider(this IServiceCollection collection)
        {
            collection.AddSingleton<ISqlConnectionStringProvider>(s =>
                        new GetSqlConnectionFromConfiguration(s.GetService<IConfiguration>(), "PowerLib"));
        }
        public static void BindPowerDataLibSqlModules(this IServiceCollection collection)
        {
            collection.AddSingleton<ISqlReader, SqlReader>();
        }
        public static void BindPowerDataLibDapperModules(this IServiceCollection collection)
        {
            collection.AddSingleton<IPowerDapperHelper, PowerDapperHelper>();
        }
    }
}
