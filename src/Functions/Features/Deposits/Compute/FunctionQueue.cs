using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using FluentValidation;
using Common.Entities;
using Common.Entities.Constants;
using Common.Entities.Validators;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Functions.Features.SharePrices.WriteThroughCache;

namespace Functions.Features.Deposits.Compute
{
    public class FunctionQueue
    {
        private readonly DepositValidator _depositValidator;
        private readonly SharePriceValidator _sharePriceValidator;

        public FunctionQueue(DepositValidator depositValidator, SharePriceValidator sharePriceValidator)
        {
            _depositValidator = depositValidator;
            _sharePriceValidator = sharePriceValidator;
        }

        [FunctionName("fn-shares-compute-queue")]
        public void Run(
            [QueueTrigger(
                QueueSettings.IncomingDepositsQueue,
                Connection = QueueSettings.ConnectionStringKey)]Deposit depositMessage,
            [Redis(key: CacheSettings.SharePricesKey, Connection = CacheSettings.ConnectionStringKey)] List<SharePriceCache> sharePrices,
            [CosmosDB(
                databaseName: DatabaseSettings.DatabaseName,
                collectionName: DatabaseSettings.DepositsContainer,
                ConnectionStringSetting = DatabaseSettings.ConnectionStringKey)]out Deposit depositDocument,
            ILogger log)
        {
            Validate(depositMessage, sharePrices);

            string transactionDate = depositMessage.TransactionDate.Substring(0, 10);
            var sharePrice = sharePrices.SingleOrDefault(sp => sp.Date == transactionDate);

            if (sharePrice is not null)
            {
                depositMessage.PartionDate = transactionDate;
                depositMessage.Shares = depositMessage.Amount / sharePrice.Price;

                depositDocument = depositMessage;
            }
            else
            {
                throw new Exception($"Share price for date {transactionDate} not found.");
            }

            void Validate(Deposit depositMessage, List<SharePriceCache> sharePrices)
            {
                var validationResult = _depositValidator.Validate(depositMessage);

                if (!validationResult.IsValid) { throw new Exception(validationResult.Errors.ToErrorsMessage()); }

                foreach (var item in sharePrices)
                {
                    validationResult = _sharePriceValidator.Validate(item);

                    if (!validationResult.IsValid) { throw new Exception(validationResult.Errors.ToErrorsMessage()); }
                }
            }
        }
    }
}
