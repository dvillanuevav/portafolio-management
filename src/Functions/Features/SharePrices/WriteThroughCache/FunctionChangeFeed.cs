using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Common.Entities;
using Common.Entities.Constants;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions.Features.SharePrices.WriteThroughCache
{
    public static class FunctionChangeFeed
    {
        [FunctionName("fn-shareprices-cache-changefeed")]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: DatabaseSettings.DatabaseName,
                collectionName: DatabaseSettings.SharePricesContainer,
                ConnectionStringSetting = DatabaseSettings.ConnectionStringKey,
                CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [Redis(key: CacheSettings.SharePricesKey, valueType: RedisValueType.Collection, Connection = CacheSettings.ConnectionStringKey)] ICollector<SharePriceCache> values,
            ILogger log)
        {
            if (input is not null && input.Count > 0)
            {
                foreach (var item in input)
                {
                    var sharePriceInserted = JsonConvert.DeserializeObject<SharePriceCache>(item.ToString());

                    values.Add(sharePriceInserted);
                }
            }
        }
    }
}
