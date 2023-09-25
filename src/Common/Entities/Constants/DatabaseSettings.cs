using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.Constants
{
    public static class DatabaseSettings
    {
        public const string ConnectionStringKey = "dbConnectionString";

        public const string DatabaseName = "Portafolio";

        public const string SharePricesContainer = "sharePrices";

        public const string DepositsContainer = "deposits";

        public const string BalancesContainer = "balances";

        public const string SharePricesPartitionKeyPath = "/partitionYear";

        public const string DepositsPartitionKeyPath = "/partitionDate";

        public const string BalancesPartitionKeyPath = "/partitionYear";
    }
}
