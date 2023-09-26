using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace API.Features.SharePrices.GetSharesPrices
{
    internal sealed class Endpoint : EndpointWithoutRequest<List<Response>>
    {
        private readonly CosmosClient _cosmosClient;

        public Endpoint(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public override void Configure()
        {
            Get("/share-prices");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Container sharePricesContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.SharePricesContainer);

            using var setIterator = sharePricesContainer.GetItemLinqQueryable<Response>(
                requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(DateTime.Now.ToString("yyyy")) })
                                    .ToFeedIterator();

            var response = await setIterator.ReadNextAsync();

            await SendAsync(response.ToList());
        }

    }
}
