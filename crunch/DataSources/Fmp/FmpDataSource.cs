using Crunch.Domain;
using Crunch.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Crunch.DataSources.Fmp
{
    /// <summary>
    /// Financial Modeling Prep API data source
    /// https://financialmodelingprep.com/developer/docs
    /// </summary>
    class FmpDataSource
    {
        private readonly string _apiKey = Env.Variables.FmpApiKey;

        /// <summary>
        /// Base url that's always the same for each API point
        /// </summary>
        private readonly string _baseUrl = "https://financialmodelingprep.com/api/v4/";

        /// <summary>
        /// Date format when converting DateTime to string
        /// </summary>
        private readonly string _dateStringFormat = "yyyy-MM-dd";

        /// <summary>
        /// Client instance for API requests
        /// </summary>
        private WebClient _client = new WebClient();



        /// <summary>
        /// Generate API url to download symbol prices from 'start' to the 'end'
        /// Url example: 
        ///     https://financialmodelingprep.com/api/v4/historical-price/AAPL/1/minute/2021-02-12/2021-02-16?apikey=
        /// </summary>
        /// <param name="symbol">stock symbol</param>
        /// <param name="interval">single price time interval</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>API url</returns>
        private string BuildPricesUrl(string symbol, PriceInterval interval, DateTime start, DateTime end)
        {
            string intervalQuery;
            if (interval == PriceInterval.OneDay)
                intervalQuery = "1/day/";

            else if (interval == PriceInterval.ThirtyMinutes)
                intervalQuery = "30/minute/";
            else
                throw new ArgumentException($"Accepted values are '1d' or '30m', not {interval}", nameof(interval));

            string startParam = start.ToString(_dateStringFormat);
            string endParam = end.ToString(_dateStringFormat);
            string query = $"historical-price/{symbol}/{intervalQuery}/{startParam}/{endParam}?apikey=";
            string url = _baseUrl + query + _apiKey;
            return url;
        }


        /// <summary>
        /// Make http get request for historical price api point
        /// </summary>
        /// <param name="url">API point url</param>
        /// <returns>JSON string of prices data</returns>
        private string RequestPricesData(string url)
        {
            string result = _client.DownloadString(url);
            return result;
        }

    }


}
