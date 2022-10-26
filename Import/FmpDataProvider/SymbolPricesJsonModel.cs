using System.Text.Json.Serialization;

namespace CrunchImport.FmpDataProvider
{
    internal class SymbolPricesJsonResponse
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
        public uint Volume { get; set; }

        /// <summary>
        /// Timestamp in format yyyy-MM-dd HH:mm:ss
        /// </summary>
        [JsonPropertyName("formated")]
        public string Timestamp { get; set; }
    }
}
