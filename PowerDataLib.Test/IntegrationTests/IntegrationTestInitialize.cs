using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerDataNet.Infrastructure;
using System;
using System.IO;

namespace PowerDataLib.Test.IntegrationTests
{
    public sealed class IntegrationTestInitialize : IDisposable
    {
        IConfiguration _configuration;
        ServiceCollection _serviceCollection;
        IServiceProvider _serviceProvider;

        public void Init()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .Build();

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddSingleton(_configuration);

            _serviceCollection.BindPowerDataLibConnectionProvider();
            _serviceCollection.BindPowerDataLibSqlModules();
            _serviceCollection.BindPowerDataLibDapperModules();

            _serviceProvider = _serviceCollection.BuildServiceProvider(new ServiceProviderOptions()
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public void Dispose()
        {
            if (_serviceProvider is ServiceProvider && _serviceProvider != null)
                (_serviceProvider as ServiceProvider).Dispose();
        }
    }
}
