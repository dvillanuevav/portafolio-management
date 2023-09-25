using API.Features.Balances.Dtos;
using API.Features.SharePrices.Dtos;
using Azure;
using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace API.Features.Balances.Endpoints
{
    public class GetGrandTotalByDate : Endpoint<BalanceRequest, BalanceResponse, BalanceMapper>
    {
        public CosmosClient CosmosClient { get; set; }

        public override void Configure()
        {
            Post("/api/balances");
            AllowAnonymous();
            Validator<BalanceRequestValidator>();
            
        }

        public override async Task HandleAsync(BalanceRequest request, CancellationToken ct)
        {
            Container balancesContainer = CosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.BalancesContainer);

            using var setIterator = balancesContainer.GetItemLinqQueryable<Balance>(requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(request.Date.Substring(0,4)) })
                                   .Where(b => b.Date == request.Date)
                                   .ToFeedIterator();

            var result = await setIterator.ReadNextAsync();

            var response = result.FirstOrDefault();

            if (response is not null) { await SendAsync(Map.FromEntity(response)); }
        }

    }
}
