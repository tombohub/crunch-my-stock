using System.Collections.Generic;
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
        public decimal Open { get; set; }

        [JsonPropertyName("h")]
        public decimal High { get; set; }

        [JsonPropertyName("c")]
        public decimal Close { get; set; }

        [JsonPropertyName("l")]
        public decimal Low { get; set; }

        [JsonPropertyName("v")]
        public long Volume { get; set; }

        /// <summary>
        /// Timestamp in format yyyy-MM-dd HH:mm:ss
        /// </summary>
        [JsonPropertyName("formated")]
        public string Timestamp { get; set; }
    }
}
