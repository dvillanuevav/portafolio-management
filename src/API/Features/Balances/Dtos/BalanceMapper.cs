using API.Features.SharePrices.Dtos;
using Common.Entities;

namespace API.Features.Balances.Dtos
{
    public class BalanceMapper : Mapper<BalanceRequest, BalanceResponse, Balance>
    {        
        public override BalanceResponse FromEntity(Balance e) => new()
        {
            Id = e.Id,
            Date = e.Date,
            GrandTotal = e.GrandTotal,
            GrandTotalShares = e.GrandTotalShares
        };
    }
}
