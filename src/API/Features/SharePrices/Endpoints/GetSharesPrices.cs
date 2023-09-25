using Common.Entities.Constants;
using Common.Entities;
using Microsoft.Azure.Cosmos;
using API.Features.SharePrices.Dtos;
using Azure.Core;
using System.Collections.Concurrent;
using Microsoft.Azure.Cosmos.Linq;

namespace API.Features.SharePrices.Endpoints
{
    public class GetSharesPrices : EndpointWithoutRequest<List<SharePriceResponse>>
    {        
        public CosmosClient CosmosClient { get; set; }

        public override void Configure()
        {
            Get("/api/share-prices");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Container sharePricesContainer = CosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.SharePricesContainer);

            using var setIterator = sharePricesContainer.GetItemLinqQueryable<SharePriceResponse>(requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(DateTime.Now.ToString("yyyy")) })                                                                        
                                    .ToFeedIterator();

            var response = await setIterator.ReadNextAsync();

            await SendAsync(response.ToList());
        }

    }
}
