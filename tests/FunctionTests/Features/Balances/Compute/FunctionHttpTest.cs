using Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionTests.Fixtures;
using Functions.Features.Balances.Compute;
using Moq;
using System.Diagnostics.Metrics;

namespace FunctionTests.Features.Balances.Compute
{
    public class FunctionHttpTest : IClassFixture<ShareDataFixture>
    {
        public readonly Mock<IDbService> _cosmosServiceMock = new();
        private readonly FunctionHttp _function;
        private readonly ShareDataFixture _shareDataFixture;
        private readonly string _balanceDate = "2023-09-23";
        private readonly string _partitionYear = "2023";
        private readonly DefaultHttpRequest _request;

        public FunctionHttpTest(ShareDataFixture fixture)
        {
            _shareDataFixture = fixture;
            _function = new FunctionHttp(_cosmosServiceMock.Object);
            _request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
            (
                new System.Collections.Generic.Dictionary<string, StringValues>()
                {
                { "balanceDate", _balanceDate }
                }
            )
            };
        }

        [Fact]
        public async Task ComputeBalance_FirstBalance_ReturnBalanceProccesed()
        {
            _cosmosServiceMock.Setup(x => x.GetCurrentBalanceByDateAsync(_balanceDate, _partitionYear)).Returns(Task.FromResult<Balance>(null));
            _cosmosServiceMock.Setup(x => x.GetPriorGrandTotalSharesBalanceAsync(_partitionYear)).Returns(Task.FromResult<decimal>(0));
            _cosmosServiceMock.Setup(x => x.GetGrandTotalSharesDepositAsync(_balanceDate)).Returns(Task.FromResult<decimal>(_shareDataFixture.Deposit.Amount));
            _cosmosServiceMock.Setup(x => x.SaveBalanceAsync(null)).Returns(Task.FromResult<Balance>(null));

            var response = await _function.Run(_request, _balanceDate, _shareDataFixture.SharePrices, _shareDataFixture.Logger);

            Assert.IsAssignableFrom<OkObjectResult>(response);

            var result = (OkObjectResult)response;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task ComputeBalance_BalanceExist_ReturnBalanceProccesed()
        {

        }

        [Fact]
        public async Task ComputeBalance_PriorBalanceExist_ReturnBalanceProccesed()
        {
        }
    }
}
