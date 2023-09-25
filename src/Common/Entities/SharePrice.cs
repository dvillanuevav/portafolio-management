using Newtonsoft.Json;

namespace Common.Entities
{
    public class SharePrice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionYear")]
        public string PartitionYear { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("_ts")]
        public int TimeSpan { get; set; }
    }
}