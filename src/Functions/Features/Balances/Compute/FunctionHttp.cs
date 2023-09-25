using Farrellsoft.Azure.Functions.Extensions.Redis;
using Common.Entities;
using Common.Entities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Functions.Features.SharePrices.WriteThroughCache;

namespace Functions.Features.Balances.Compute
{
    public class FunctionHttp
    {
        private readonly IBalanceService _balanceStrategy;
        public FunctionHttp(IDbService cosmosRepository)
        {
            _balanceStrategy = new BalanceFowardService(cosmosRepository);
        }

        [FunctionName("fn-balance-compute-http")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "compute-balance-adhoc/{balanceDate}")] HttpRequest req, string balanceDate,
            [Redis(key: CacheSettings.SharePricesKey, Connection = CacheSettings.ConnectionStringKey)] List<SharePriceCache> sharePrices,
            ILogger log)
        {

            var sharePrice = sharePrices.SingleOrDefault(sp => sp.Date == balanceDate);

            if (sharePrice is not null)
            {
                var balanceCalculated = await _balanceStrategy.ComputeAsync(balanceDate, sharePrice);

                log.LogInformation($"Balance's strategy executed with results: {JsonSerializer.Serialize<Balance>(balanceCalculated)}");

                return new OkObjectResult(balanceCalculated);
            }

            return new BadRequestObjectResult($"Share price for date {balanceDate} not found.");
        }
    }
}
