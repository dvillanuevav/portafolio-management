using Common.Entities.Constants;
using Common.Entities.Validators;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using System;

namespace API.Startup
{
    public static partial class ServiceInitializer
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddFastEndpoints();            
            services.AddSingleton(AddDatabase(configuration).GetAwaiter().GetResult());

            return services;
        }

        public static async Task<CosmosClient> AddDatabase(IConfiguration configuration)
        {
            var connectionString = configuration[DatabaseSettings.ConnectionStringKey];

            var cosmosClient = new CosmosClientBuilder(connectionString).Build();

            await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseSettings.DatabaseName);

            var database = cosmosClient.GetDatabase(DatabaseSettings.DatabaseName);

            await database.CreateContainerIfNotExistsAsync(DatabaseSettings.SharePricesContainer, DatabaseSettings.SharePricesPartitionKeyPath);
            await database.CreateContainerIfNotExistsAsync(DatabaseSettings.BalancesContainer, DatabaseSettings.BalancesPartitionKeyPath);
            await database.CreateContainerIfNotExistsAsync(DatabaseSettings.DepositsContainer, DatabaseSettings.DepositsPartitionKeyPath);

            return cosmosClient;
        }

        public static IApplicationBuilder AddApplication(this IApplicationBuilder app)
        {
            app.UseFastEndpoints();
            
            return app;
        }
    }
}
