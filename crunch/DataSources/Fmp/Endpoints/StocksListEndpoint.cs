using Crunch.DataSources.Fmp.Responses;
using System.Net;
using System.Text.Json;

namespace Crunch.DataSources.Fmp.Endpoints
{
    /// <summary>
    /// Stocks ticker symbols available in Financial Modeling Prep
    /// </summary>
    class StocksListEndpoint
    {
        /// <summary>
        /// query part of the API URL
        /// </summary>
        private readonly string _query = "api/v3/stock-only/list/";

        /// <summary>
        /// Client instance for API requests
        /// </summary>
        private readonly WebClient _webClient = new();

        /// <summary>
        /// Complete URL for API request
        /// </summary>
        private string Url
        {
            get
            {
                return UrlBuilder.BuildUrl(_query);
            }
        }

        /// <summary>
        /// Get stocks available on Financial Modeling Prep
        /// </summary>
        /// <returns>List of stocks JSON response</returns>
        public SymbolsListResponse GetStocks()
        {
            string result = _webClient.DownloadString(Url);
            var symbolsList = JsonSerializer.Deserialize<SymbolsListResponse>(result);
            return symbolsList;
        }
    }
}
