using System.Text.Json.Serialization;

namespace CrunchImport.DataProviders.Fmp.Responses
{
    internal record SymbolsListJsonModel
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("price")]
        public decimal Price { get; init; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; init; }

        [JsonPropertyName("exchangeShortName")]
        public string ExchangeShortName { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }
    }
}