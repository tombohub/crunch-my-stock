using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Crunch.DataSources.Fmp.HistoricalPricesEndpoint
{
    /// <summary>
    /// Represents class model for JSON response
    /// </summary>
    internal class HistoricalPricesJsonModel
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("results")]
        public List<HistoricalPricesItem> Results { get; set; }
    }

    internal class HistoricalPricesItem
    {
        [JsonPropertyName("o")]
        public double Open { get; set; }

        [JsonPropertyName("h")]
        public double High { get; set; }

        [JsonPropertyName ("c")]
        public double Close { get; set; }

        [JsonPropertyName("l")]
        public double Low { get; set; }
        
        [JsonPropertyName("v")]
        public long Volume { get; set; }

        /// <summary>
        /// Timestamp in format yyyy-mm-dd hh:mm:ss
        /// </summary>
        [JsonPropertyName("formated")]
        public string Timestamp { get; set; }
    }
}
