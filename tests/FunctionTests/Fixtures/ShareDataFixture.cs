using Common.Entities;
using Functions.Features.SharePrices.WriteThroughCache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTests.Fixtures
{
    public class ShareDataFixture
    {
        public readonly List<SharePriceCache> SharePrices = new List<SharePriceCache> { new SharePriceCache { Date = "2023-09-23", Price = 1200m } };
        
        public readonly ILogger Logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

        public readonly Deposit Deposit = new Deposit{ Amount = 100000m, TransactionDate = "2023-09-23T13:33:03Z" };
}
}
