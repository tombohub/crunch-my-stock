using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.DataSources.Fmp
{
    /// <summary>
    /// Builds the URL for the API request
    /// </summary>
    class UrlBuilder
    {
        /// <summary>
        /// Api key part of the URL.
        /// </summary>
        private static readonly string _apikey = "?apikey=" + Env.Variables.FmpApiKey;

        /// <summary>
        /// Domain part of the URL
        /// </summary>
        private static readonly string _domain = "https://financialmodelingprep.com/";

        /// <summary>
        /// Build complete URL for API request
        /// </summary>
        /// <param name="query">Query part of the URL. Different for each endpoint</param>
        /// <returns>Complete URL ready for HTTP request</returns>
        public static string BuildUrl(string query)
        {
            string url = _domain + query + _apikey;
            return url;
        }
    }
}
