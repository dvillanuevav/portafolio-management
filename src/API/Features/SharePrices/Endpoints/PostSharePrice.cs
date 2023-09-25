using API.Features.SharePrices.Dtos;
using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;
using System;

namespace API.Features.SharePrices.Endpoints
{
    public class PostSharePrice : Endpoint<SharePriceRequest, SharePriceResponse, SharePriceMapper>
    {
        public CosmosClient CosmosClient { get; set; }

        public override void Configure()
        {
            Post("/api/share-prices");
            AllowAnonymous();
            Validator<SharePriceRequestValidator>();
        }

        public override async Task HandleAsync(SharePriceRequest request, CancellationToken ct)
        {
            SharePrice entity = Map.ToEntity(request);

            Container sharePricesContainer = CosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.SharePricesContainer);

            var result = await sharePricesContainer.CreateItemAsync(entity, partitionKey: new PartitionKey(request.Date.Substring(0, 4)));

            await SendAsync(Map.FromEntity(result.Resource));
        }
    }
}
