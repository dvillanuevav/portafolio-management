using Common.Entities;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Features.SharePrices.WriteThroughCache
{
    public class SharePriceCache : SharePrice, IRedisListItem
    {
    }        
}
