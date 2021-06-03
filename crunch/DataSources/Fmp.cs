using Crunch;
using System;

namespace Crunch.DataSources
{
    /// <summary>
    /// Financial Modeling Prep API data source
    /// https://financialmodelingprep.com/developer/docs
    /// </summary>
    class Fmp     
    {
        private readonly string _apiKey = Env.Variables.FmpApiKey;
        private readonly string _baseUrl = "https://financialmodelingprep.com/api/v4/";
        private readonly string _today = DateTime.Today.ToShortDateString();

        /// <summary>
        /// Generate API url to download symbol prices from `start` to latest price
        /// </summary>
        /// <param name="symbol">stock symbol</param>
        /// <param name="interval">price bar interval, 1d or 30m</param>
        /// <param name="start">start date in format yyyy-mm-dd</param>
        /// <returns>API url</returns>
        public string _generatePricesUrl(string symbol, string interval, string start)
        {
            string end = _today;
            string intervalQuery;

            if (interval == "1d")
            {
                intervalQuery = "1/day/";
            }
            else if (interval == "30m")
            {
                intervalQuery = "30/minute/";
            }
            else
            {
                throw new ArgumentException($"Accepted values are '1d' or '30m', not {interval}", nameof(interval));
            }

            string query = $"historical-price/{symbol}/{intervalQuery}/{start}/{end}?apikey=";
            string url = _baseUrl + query + _apiKey;

            return url;
        }

        /// <summary>
        /// Generate API url to download symbol prices from 'start' to the 'end'
        /// </summary>
        /// <param name="symbol">stock symbol</param>
        /// <param name="interval">price bar interval, 1d or 30m</param>
        /// <param name="start">start date in format yyyy-mm-dd</param>
        /// <param name="end">end date in format yyyy-mm-dd</param>
        /// <returns>API url</returns>
        public string _generatePricesUrl(string symbol, string interval, string start, string end)
        {
            string intervalQuery;

            if (interval == "1d")
            {
                intervalQuery = "1/day/";
            }
            else if (interval == "30m")
            {
                intervalQuery = "30/minute/";
            }
            else
            {
                throw new ArgumentException($"Accepted values are '1d' or '30m', not {interval}", nameof(interval));
            }

            string query = $"historical-price/{symbol}/{intervalQuery}/{start}/{end}?apikey=";
            string url = _baseUrl + query + _apiKey;

            return url;
        }

    }
}
