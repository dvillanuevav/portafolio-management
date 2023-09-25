using Common.Entities;
using Common.Entities.Validators;
using Functions.Features.Deposits.Compute;
using Functions.Features.SharePrices.WriteThroughCache;
using FunctionTests.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections;

namespace FunctionTests.Features.Deposits.Compute
{
    public class FunctionQueueTest : IClassFixture<ShareDataFixture>
    {       
        private readonly FunctionQueue _function = new FunctionQueue(new DepositValidator(), new SharePriceValidator());

        private readonly ShareDataFixture _shareDataFixture;

        public FunctionQueueTest(ShareDataFixture fixture)
        {
            _shareDataFixture = fixture;
        }

        [Fact]
        public void ComputeShares_IsDepositValid_ReturnDepositProccesed()
        {
            // Arrange
            var depositMessage = _shareDataFixture.Deposit;
            var sharesExpected = (decimal) depositMessage.Amount / _shareDataFixture.SharePrices.First().Price;


            // Act
            _function.Run(depositMessage, _shareDataFixture.SharePrices, out Deposit depositDocument, _shareDataFixture.Logger);

            // Assert
            Assert.NotNull(depositDocument);
            Assert.Equal(sharesExpected, depositDocument.Shares);
        }       

        [Theory]
        [MemberData(nameof(TestDataGenerator.GeDepositFromDataGenerator), MemberType = typeof(TestDataGenerator))]
        public void ComputeShares_DepositMissingValues_ThrowException(Deposit depositMessage)
        {
            // Act
            Action act = () => _function.Run(depositMessage, _shareDataFixture.SharePrices, out Deposit depositDocument, _shareDataFixture.Logger);

            // Assert            
            var ex = Assert.Throws<Exception>(act);
            Assert.Contains("must be", ex.Message);
        }

        [Fact]
        public void ComputeShares_SharePriceNotFound_ThrowException()
        {
            // Arrange
            var depositMessage = _shareDataFixture.Deposit;
            var sharePricesEmpty = new List<SharePriceCache>();

            // Act
            Action act = () => _function.Run(depositMessage, sharePricesEmpty, out Deposit depositDocument, _shareDataFixture.Logger);

            // Assert            
            var ex = Assert.Throws<Exception>(act);
            Assert.Equal("Share price for date 2023-09-23 not found.", ex.Message);
        }
    }

    public class TestDataGenerator : IEnumerable<object[]>
    {
        public static IEnumerable<object[]> GeDepositFromDataGenerator()
        {
            yield return new object[] { new Deposit() };
            yield return new object[] { new Deposit { Amount = 100000m } };
            yield return new object[] { new Deposit { TransactionDate = "2023-09-23T13:33:03Z" } };
            yield return new object[] { new Deposit { Amount = 0m, TransactionDate = "2023-09-23T13:33:03Z" } };
            yield return new object[] { new Deposit { TransactionDate = "" } };
            yield return new object[] { new Deposit { TransactionDate = "2023-09-23" } };
            yield return new object[] { new Deposit { TransactionDate = "NotValid" } };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}