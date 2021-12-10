using Crunch.DataSources.Fmp.Responses;
using System.Net;
using System.Text.Json;


namespace Crunch.DataSources.Fmp.Endpoints
{
    /// <summary>
    /// All tradable Symbols
    /// </summary>
    class TradableSymbolsListEndpoint
    {
        /// <summary>
        /// query part of the API URL
        /// </summary>
        private readonly string _query = "api/v3/available-traded/list";

        /// <summary>
        /// Client instance for API requests
        /// </summary>
        private readonly WebClient _webClient = new();

        /// <summary>
        /// Complete URL for API request
        /// </summary>
        private string Url
        {
            get { return UrlBuilder.BuildUrl(_query); }
        }

        /// <summary>
        /// All securities available for trading
        /// </summary>
        /// <returns>List of securities data</returns>
        public SymbolsListResponse GetSymbols()
        {
            string result = _webClient.DownloadString(Url);
            var symbols = JsonSerializer.Deserialize<SymbolsListResponse>(result);
            return symbols;
        }

    }
}
