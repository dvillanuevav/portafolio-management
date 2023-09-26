using Common.Entities;

namespace API.Features.Balances.GetBalanceByDate
{
    internal sealed class Mapper : Mapper<Request, Response, Balance>
    {
        public override Response FromEntity(Balance e) => new()
        {
            Id = e.Id,
            Date = e.Date,
            GrandTotal = e.GrandTotal,
            GrandTotalShares = e.GrandTotalShares
        };
    }
}