using Newtonsoft.Json;

namespace Common.Entities
{
    public class Deposit
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionDate")]
        public string PartionDate { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("transactionDate")]
        public string TransactionDate { get; set; }

        [JsonProperty("shares")]
        public decimal Shares { get; set; }
    }
}