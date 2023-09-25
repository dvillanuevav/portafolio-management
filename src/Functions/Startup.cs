using Functions;
using Common.Entities.Constants;
using Common.Entities.Validators;
using Functions.Features.Balances.Compute;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Functions
{
    public class Startup : FunctionsStartup
    {       
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<DepositValidator>();
            builder.Services.AddTransient<SharePriceValidator>();
            builder.Services.AddTransient<IDbService, CosmosService>();                       

            builder.Services.AddSingleton(s => {
                var connectionString = builder.GetContext().Configuration[DatabaseSettings.ConnectionStringKey];
                return new CosmosClientBuilder(connectionString)
                    .Build();
            });
        }
    }
}
