using Common.Entities;
using System.Threading.Tasks;

namespace Functions.Features.Balances.Compute
{
    public interface IDbService
    {
        public Task<Balance> SaveBalanceAsync(Balance newBalance);

        public Task<Balance> GetCurrentBalanceByDateAsync(string balanceDate, string partitionYear);

        public Task<decimal> GetPriorGrandTotalSharesBalanceAsync(string partitionYear);

        public Task<decimal> GetGrandTotalSharesDepositAsync(string balanceDate);
    }
}
