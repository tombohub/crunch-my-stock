using System.Text.Json.Serialization;

namespace Crunch.DataSources.Fmp
{
    internal class ErrorResponseJsonModel
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
