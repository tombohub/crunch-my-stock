using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Crunch.DataSources.Fmp
{
    internal class ErrorResponseJsonModel
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
