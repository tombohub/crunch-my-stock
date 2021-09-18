using Crunch.Domain;
using Crunch.Domain.OhlcPrice;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Crunch.DataSources
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

        /// <summary>
        /// Maps the JSON string prices data to Price entity object
        /// </summary>
        /// <param name="json">Prices data in JSON format</param>
        /// <param name="interval">Time interval of price</param>
        /// <returns>List of Price objects</returns>
        private List<Price> JsonToPriceObject(string json, PriceInterval interval)
        {
            using (var document = JsonDocument.Parse(json))
            {
                var root = document.RootElement;
                var symbol = root.GetProperty("symbol").GetString();
                var pricesJsonData = root.GetProperty("results");
                var prices = new List<Price>();
                foreach (var priceJsonData in pricesJsonData.EnumerateArray())
                {
                    var price = new Price(
                        symbol: symbol,
                        timestamp: DateTime.Parse(priceJsonData.GetProperty("formated").GetString()),
                        open: priceJsonData.GetProperty("o").GetDouble(),
                        high: priceJsonData.GetProperty("h").GetDouble(),
                        low: priceJsonData.GetProperty("l").GetDouble(),
                        close: priceJsonData.GetProperty("c").GetDouble(),
                        volume: priceJsonData.GetProperty("v").GetUInt64(),
                        interval: interval
                        );
                    prices.Add(price);
                }

                return prices;
            }

        }

        /// <summary>
        /// Get historical prices data from start till now
        /// </summary>
        /// <param name="symbol">security symbol</param>
        /// <param name="interval">price time interval</param>
        /// <param name="start">start date to get prices from</param>
        /// <param name="end">end date to get prices to</param>
        /// <returns></returns>
        public List<Price> GetPrices(string symbol, PriceInterval interval, DateTime start, DateTime end)
        {
            string url = BuildPricesUrl(symbol, interval, start, end);
            string json = RequestPricesData(url);
            var prices = JsonToPriceObject(json, interval);
            return prices;
        }

    }


}
