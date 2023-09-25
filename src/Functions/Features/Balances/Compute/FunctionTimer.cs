using System;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using System.Collections.Generic;
using Common.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Common.Entities.Constants;
using Functions.Features.SharePrices.WriteThroughCache;

namespace Functions.Features.Balances.Compute
{
    public class FunctionTimer
    {

        private readonly BalanceFowardService _strategy;
        public FunctionTimer(BalanceFowardService strategy)
        {
            _strategy = strategy;
        }

        [FunctionName("fn-balance-compute-timer")]
        public async Task Run(
            [TimerTrigger("0 0 22 * * *")] TimerInfo myTimer,
            [Redis(key: CacheSettings.SharePricesKey, Connection = CacheSettings.ConnectionStringKey)] List<SharePriceCache> sharePrices,
            ILogger log)
        {
            var executionDate = DateTime.Now.ToString("yyyy-MM-dd");

            var sharePrice = sharePrices.SingleOrDefault(sp => sp.Date == executionDate);

            var balanceCalculated = await _strategy.ComputeAsync(executionDate, sharePrice);            

            log.LogInformation($"Balance's strategy executed with results: {JsonSerializer.Serialize<Balance>(balanceCalculated)}");
        }
    }
}
