using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrunchImport.DataProviders.Fmp.Responses
{
    /// <summary>
    /// Class representation of FMP json response
    /// for requesting historical symbol prices
    /// </summary>
    internal class SymbolPricesJsonResponse
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// Number of results in list of OHLCs
        /// </summary>
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }

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