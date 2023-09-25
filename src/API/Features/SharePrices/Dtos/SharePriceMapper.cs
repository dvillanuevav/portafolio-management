using Common.Entities;
using System;

namespace API.Features.SharePrices.Dtos
{
    public class SharePriceMapper : Mapper<SharePriceRequest, SharePriceResponse, SharePrice>
    {
        public override SharePrice ToEntity(SharePriceRequest r) => new()
        {
            Price = r.Price,
            Date = r.Date,
            PartitionYear = r.Date.Substring(0,4),
            Id = Guid.NewGuid().ToString()
        };

        public override SharePriceResponse FromEntity(SharePrice e) => new()
        {
            Id = e.Id,
            Price = e.Price,
            Date = e.Date,
        };
    }
}
