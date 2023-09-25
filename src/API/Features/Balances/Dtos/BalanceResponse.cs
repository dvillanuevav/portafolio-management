using Newtonsoft.Json;

namespace API.Features.Balances.Dtos
{
    public class BalanceResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }       

        [JsonProperty("date")]
        public string Date { get; init; }

        [JsonProperty("grandTotalShares")]
        public decimal GrandTotalShares { get; set; }

        [JsonProperty("grandTotal")]
        public decimal GrandTotal { get; set; }
    }
}
