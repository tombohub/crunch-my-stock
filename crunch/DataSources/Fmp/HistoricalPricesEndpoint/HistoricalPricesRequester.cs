using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Crunch.Domain;
using Crunch.Domain.OhlcPrice;

namespace Crunch.DataSources.Fmp.HistoricalPricesEndpoint
{
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

        public HistoricalPricesRequester()
        {
            
        }

        public PriceSet RequestData(string symbol, PriceInterval interval, DateOnly start, DateOnly end)
        {
            string queryString = CreateQueryString(symbol, interval, start, end);
            string apiUrl = UrlBuilder.BuildUrl(queryString);
            string apiResponse = _webClient.DownloadString(apiUrl);
            HistoricalPricesJsonModel jsonPricesData = JsonSerializer.Deserialize<HistoricalPricesJsonModel>(apiResponse);
            PriceSet priceSet = MapJsonToPriceSet(symbol, interval, jsonPricesData);

            return priceSet;
        }

        private PriceSet MapJsonToPriceSet(string symbol, PriceInterval interval, HistoricalPricesJsonModel jsonPricesData)
        {
            var prices = new List<Price>();
            foreach (var jsonPrice in jsonPricesData.Results)
            {
                DateTime timestamp = DateTime.Parse(jsonPrice.Timestamp);
                var price = new Price(
                    Timestamp: timestamp,
                    Open: jsonPrice.Open,
                    High: jsonPrice.High,
                    Low: jsonPrice.Low,
                    Close: jsonPrice.Close,
                    Volume: jsonPrice.Volume
                    );
                prices.Add(price);
            }
            var priceSet = new PriceSet(
                Symbol: symbol,
                interval: interval,
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
        private string CreateQueryString(string symbol, PriceInterval interval, DateOnly start, DateOnly end)
        {
            string intervalQuery = interval switch
            {
                PriceInterval.OneDay => "1/day",
                PriceInterval.ThirtyMinutes => "30/minute",
                _ => throw new ArgumentException($"Interval {interval} doesn't Exist")
            };

            // HACK: string templating
            string queryString = _queryTemplate
                .Replace("{symbol}", symbol)
                .Replace("{interval}", intervalQuery)
                .Replace("{start}", start.ToString())
                .Replace("{end}", end.ToString());

            return queryString;
        }



    }
}
