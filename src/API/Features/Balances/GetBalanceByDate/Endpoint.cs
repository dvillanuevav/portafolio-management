using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace API.Features.Balances.GetBalanceByDate
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
            Get("/balances/{date}");
            AllowAnonymous();
            Validator<Validator>();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            Container balancesContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.BalancesContainer);

            using var setIterator = balancesContainer.GetItemLinqQueryable<Balance>(
                requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(r.Date.Substring(0, 4)) })
                                   .Where(b => b.Date == r.Date)
                                   .ToFeedIterator();

            var result = await setIterator.ReadNextAsync();

            var response = result.FirstOrDefault();

            if (response is null)
            {
                await SendNotFoundAsync();
            }
            else
            {
                await SendAsync(Map.FromEntity(response));
            }
        }
    }
}