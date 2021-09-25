using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using Crunch.DataSources.Fmp.Responses;

namespace Crunch.DataSources.Fmp.Endpoints
{
    /// <summary>
    /// ETFs ticker symbols available in Financial Modeling Prep
    /// </summary>
    class EtfListEndpoint
    {
        /// <summary>
        /// query part of the API URL
        /// </summary>
        private readonly string _query = "api/v3/etf/list/";

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
        /// Get available ETFs on Financial Modeling Prep
        /// </summary>
        /// <returns>List of Etfs</returns>
        public SymbolsListResponse GetEtfs()
        {
            string result = _webClient.DownloadString(Url);
            var etfs = JsonSerializer.Deserialize<SymbolsListResponse>(result);
            return etfs;

        }
    }
}
