using Newtonsoft.Json;

namespace API.Features.SharePrices.Dtos
{
    public class SharePriceResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
