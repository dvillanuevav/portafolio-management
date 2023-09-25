using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Features.Balances.Compute
{
    public class CosmosService : IDbService
    {
        private readonly CosmosClient _cosmosClient;


        public CosmosService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<Balance> SaveBalanceAsync(Balance newBalance)
        {            
            Container balanceContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.BalancesContainer);

            var response = await balanceContainer.UpsertItemAsync(newBalance, partitionKey: new PartitionKey(newBalance.PartitionYear));

            return response.Resource;
        }

        public async Task<Balance> GetCurrentBalanceByDateAsync(string balanceDate, string partitionYear)
        {
            Container balanceContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.BalancesContainer);

            using var setIterator = balanceContainer.GetItemLinqQueryable<Balance>(requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(partitionYear) })
                                    .Where(b => b.Date == balanceDate)
                                    .ToFeedIterator();

            var response = await setIterator.ReadNextAsync();

            return response.FirstOrDefault();
        }

        public async Task<decimal> GetPriorGrandTotalSharesBalanceAsync(string partitionYear)
        {
            Container balanceContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.BalancesContainer);

            using var setIterator = balanceContainer.GetItemLinqQueryable<Balance>(requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(partitionYear) })
                                                .OrderByDescending(b => b.TimeSpan)
                                                .Take(1)
                                                .Select(b => b.GrandTotalShares)
                                                .ToFeedIterator();

            var response = await setIterator.ReadNextAsync();

            return response.FirstOrDefault();
        }

        public async Task<decimal> GetGrandTotalSharesDepositAsync(string balanceDate)
        {
            Container depositContainer = _cosmosClient.GetContainer(DatabaseSettings.DatabaseName, DatabaseSettings.DepositsContainer);

            var response = await depositContainer.GetItemLinqQueryable<Deposit>(requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(balanceDate) })
                                    .Select(d => d.Shares)
                                    .SumAsync();

            return response.Resource;
        }
    }
}
