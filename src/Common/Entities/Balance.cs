using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Balance
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionYear")]
        public string PartitionYear { get; init; }

        [JsonProperty("date")]
        public string Date { get; init; }

        [JsonProperty("grandTotalShares")]
        public decimal GrandTotalShares { get; set; }

        [JsonProperty("grandTotal")]
        public decimal GrandTotal { get; set; }

        [JsonProperty("_ts")]
        public int TimeSpan { get; set; }
    }
}
