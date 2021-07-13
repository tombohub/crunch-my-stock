using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Crunch.DataSource
{

    /// <summary>
    /// Class representation of FMP historical prices JSON data
    /// </summary>
    public class HistoricalPrices
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("totalResults")]
        public uint TotalResults { get; set; }

        [JsonPropertyName("results")]
        public List<Price> Prices { get; set; }

    }

    public class Price
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
        public ulong Volume { get; set; }

        [JsonPropertyName("formated")]
        public string Timestamp { get; set; }
    }
}
