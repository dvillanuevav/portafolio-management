using Common.Entities;

namespace API.Features.SharePrices
{
    internal sealed class Mapper : Mapper<Request, Response, SharePrice>
    {
        public override SharePrice ToEntity(Request r) => new()
        {
            Price = r.Price,
            Date = r.Date,
            PartitionYear = r.Date.Substring(0, 4),
            Id = Guid.NewGuid().ToString()
        };

        public override Response FromEntity(SharePrice e) => new()
        {
            Id = e.Id,
            Price = e.Price,
            Date = e.Date,
        };
    }

}
