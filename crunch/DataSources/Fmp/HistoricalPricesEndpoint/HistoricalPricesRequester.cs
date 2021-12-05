using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Crunch.Domain;

namespace Crunch.DataSources.Fmp.HistoricalPricesEndpoint
{
    /// <summary>
    /// Responsible for requesting prices data from Financial Modeling Prep data source
    /// </summary>
    internal class HistoricalPricesRequester
    {
        /// <summary>
        /// query part template of the API URL
        /// </summary>
        /// <example>/api/v4/historical-price/tecs/1/day/2021-10-22/2021-11-28</example>
        private readonly string _queryTemplate = "/api/v4/historical-price/{symbol}/{interval}/{start}/{end}";

        /// <summary>
        /// Client instance for API requests
        /// </summary>
        //TODO: repeated field, make it dry
        private readonly WebClient _webClient = new();

        /// <summary>
        /// Make Api request from historical prices data
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DailyPriceSet RequestData(string symbol, TimeRange timeRange, PriceInterval interval)
        {
            string queryString = CreateQueryString(symbol, timeRange, interval);
            string apiUrl = UrlBuilder.BuildUrl(queryString);
            string apiResponse = _webClient.DownloadString(apiUrl);
            HistoricalPricesJsonModel jsonPricesData = JsonSerializer.Deserialize<HistoricalPricesJsonModel>(apiResponse);
            DailyPriceSet priceSet = MapJsonToPriceSet(symbol, interval, jsonPricesData);
            return priceSet;
        }

        /// <summary>
        /// Helper to map the json data model to the domain price set object
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="jsonPricesData"></param>
        /// <returns></returns>
        private DailyPriceSet MapJsonToPriceSet(string symbol, PriceInterval interval, HistoricalPricesJsonModel jsonPricesData)
        {
            var prices = new List<PriceDaily>();
            foreach (var jsonPrice in jsonPricesData.Results)
            {
                DateOnly timestamp = DateOnly.Parse(jsonPrice.Timestamp);
                var price = new PriceDaily(
                    Timestamp: timestamp,
                    Open: jsonPrice.Open,
                    High: jsonPrice.High,
                    Low: jsonPrice.Low,
                    Close: jsonPrice.Close,
                    Volume: jsonPrice.Volume
                    );
                prices.Add(price);
            }
            var priceSet = new DailyPriceSet(
                Symbol: symbol,
                Interval: interval,
                Prices: prices
                );

            return priceSet;
        }

        /// <summary>
        /// Helper method to create query part of the Url for API request.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string CreateQueryString(string symbol, TimeRange timeRange, PriceInterval interval)
        {
            string intervalQuery = interval switch
            {
                PriceInterval.OneDay => "1/day",
                PriceInterval.ThirtyMinutes => "30/minute",
                _ => throw new ArgumentException($"Interval {interval} doesn't Exist")
            };

            string start = timeRange.Start.ToShortDateString();
            string end = timeRange.End.ToShortDateString();

            // HACK: string templating
            string queryString = _queryTemplate
                .Replace("{symbol}", symbol)
                .Replace("{interval}", intervalQuery)
                .Replace("{start}", start)
                .Replace("{end}", end);

            return queryString;
        }



    }
}
