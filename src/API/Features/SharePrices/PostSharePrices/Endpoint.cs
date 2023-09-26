using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;

namespace API.Features.SharePrices.PostSharePrices
{
    internal sealed class Endpoint : Endpoint<Request, Response, Mapper>
    {
        private readonly CosmosClient _cosmosClient;

        public Endpoint(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public override void Configure()
        {
            Post("/share-prices");
            AllowAnonymous();
            Validator<Validator>();
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            SharePrice entity = Map.ToEntity(request);

            Container sharePricesContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.SharePricesContainer);

            var result = await sharePricesContainer.CreateItemAsync(entity, partitionKey: new PartitionKey(request.Date.Substring(0, 4)));

            await SendAsync(Map.FromEntity(result.Resource));
        }
    }
}
