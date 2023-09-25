using Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Linq;
using Common.Entities.Constants;
using Functions.Features.SharePrices.WriteThroughCache;

namespace Functions.Features.Balances.Compute
{
    public class BalanceFowardService : IBalanceService
    {
        private readonly IDbService _cosmosRepository;

        public BalanceFowardService(IDbService cosmosRepository)
        {
            _cosmosRepository = cosmosRepository;
        }

        public async Task<Balance> ComputeAsync(string balanceDate, SharePriceCache sharePrice)
        {
            var currentBalance = await _cosmosRepository.GetCurrentBalanceByDateAsync(balanceDate, sharePrice.PartitionYear);

            decimal priorGrandTotalShares = default(decimal);
            if (currentBalance is null || (currentBalance is not null && currentBalance.Date != balanceDate))
            {
                priorGrandTotalShares = await _cosmosRepository.GetPriorGrandTotalSharesBalanceAsync(sharePrice.PartitionYear);
            }

            decimal newGrandTotalShares = await _cosmosRepository.GetGrandTotalSharesDepositAsync(balanceDate);

            var newBalance = currentBalance ?? new Balance
            {
                PartitionYear = sharePrice.PartitionYear,
                Date = balanceDate,
                Id = Guid.NewGuid().ToString()
            };

            newBalance.GrandTotalShares = priorGrandTotalShares + newGrandTotalShares;
            newBalance.GrandTotal = newBalance.GrandTotalShares * sharePrice.Price;            

            return await _cosmosRepository.SaveBalanceAsync(newBalance);
        }        
    }
}
