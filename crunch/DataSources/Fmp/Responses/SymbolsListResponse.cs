using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crunch.DataSources.Fmp.Responses
{
    class SymbolsListResponse : List<SymbolsListItem>
    {
    }

    record SymbolsListItem
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }
    }
}
