using Common.Entities;
using Functions.Features.SharePrices.WriteThroughCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Features.Balances.Compute
{
    public interface IBalanceService
    {
        public Task<Balance> ComputeAsync(string balanceDate, SharePriceCache sharePrice);        
    }
}
