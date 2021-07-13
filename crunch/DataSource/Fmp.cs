using System;
using System.Net;
using System.Collections.Generic;
using System.Text.Json;

namespace Crunch.DataSource
{
    /// <summary>
    /// Financial Modeling Prep API data source
    /// https://financialmodelingprep.com/developer/docs
    /// </summary>
    class Fmp
    {
        private readonly string _apiKey = Env.Variables.FmpApiKey;

        /// <summary>
        /// Base url that's always the same for each API point
        /// </summary>
        private readonly string _baseUrl = "https://financialmodelingprep.com/api/v4/";

        /// <summary>
        /// Today's date in yyyy-mm-dd format
        /// </summary>
        private readonly string _today = DateTime.Today.ToShortDateString();

        /// <summary>
        /// Client instance for API requests and getting results
        /// </summary>
        private WebClient _client = new WebClient();



        /// <summary>
        /// Generate API url to download symbol prices from 'start' to the 'end'
        /// Url example: 
        ///     https://financialmodelingprep.com/api/v4/historical-price/AAPL/1/minute/2021-02-12/2021-02-16?apikey=
        /// </summary>
        /// <param name="symbol">stock symbol</param>
        /// <param name="interval">price bar interval, 1d or 30m</param>
        /// <param name="start">start date in format yyyy-mm-dd</param>
        /// <param name="end">end date in format yyyy-mm-dd</param>
        /// <returns>API url</returns>
        private string _generatePricesUrl(string symbol, string interval, string start, string end)
        {
            string intervalQuery;

            if (interval == "1d")
                intervalQuery = "1/day/";
            else if (interval == "30m")
                intervalQuery = "30/minute/";
            else
                throw new ArgumentException($"Accepted values are '1d' or '30m', not {interval}", nameof(interval));

            string query = $"historical-price/{symbol}/{intervalQuery}/{start}/{end}?apikey=";
            string url = _baseUrl + query + _apiKey;

            return url;
        }


        /// <summary>
        /// Make http get request for historical price api point
        /// </summary>
        /// <param name="url">API point url</param>
        /// <returns>JSON string of prices data</returns>
        private string _requestPricesData(string url)
        {
            string result = _client.DownloadString(url);
            return result;
        }


        private HistoricalPrices _deserializeJSON(string json)
        {
            var ko = JsonSerializer.Deserialize<HistoricalPrices>(json);
            return ko;
        }

        public string GetPrices(string symbol, string interval, string start, string end)
        {
            string url = _generatePricesUrl(symbol, interval, start, end);
            string data = _requestPricesData(url);
            return data;
        }

        public HistoricalPrices GetPrices(string symbol, string interval, string start)
        {
            string end = _today;
            string url = _generatePricesUrl(symbol, interval, start, end);
            string json = _requestPricesData(url);
            var price = _deserializeJSON(json);
            return price;

        }

        public string test()
        {
            return "kokolo";
        }

    }


}
