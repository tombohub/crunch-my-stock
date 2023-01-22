using System.Text.Json;
using Crunch.Domain;
using CrunchImport.DataProviders.Fmp.Responses;

namespace CrunchImport.DataProviders.Fmp
{
    internal class FmpProvider
    {
        private const string _fmpApiKey = "***REMOVED***";
        private const string _fmpDomain = "https://financialmodelingprep.com/";
        private HttpClient _httpClient = new HttpClient();

        public SecurityPrice GetSecurityDailyPrice(Symbol symbol, TradingDay tradingDay)
        {
            string priceApiResponse = RequestDailyPrice(symbol.Value, tradingDay.Date.ToString("yyyy-MM-dd"));
            var price = JsonSerializer.Deserialize<SymbolPricesJsonResponse>(priceApiResponse);
            if (price.Results.Count > 1)
            {
                throw new Exception("There's more than one result from security daily price request");
            }
            return new SecurityPrice
            {
                Symbol = symbol,
                Date = tradingDay,
                Price = new OHLC(
                    price.Results[0].Open,
                    price.Results[0].High,
                    price.Results[0].Low,
                    price.Results[0].Close
                                ),
                Volume = price.Results[0].Volume,
            };
        }

        /// <summary>
        /// Send a request for symbol prices on particular day
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="date">date to get prices for</param>
        /// <returns>Day price data in OHLC format</returns>
        private string RequestDailyPrice(string symbol, string date)
        {
            var response = RequestDailyPrice(symbol: symbol, startDate: date, endDate: date);
            return response;
        }

        /// <summary>
        /// Send a request for the daily price of particular security for period between start and end date
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="startDate">first day to get prices data</param>
        /// <param name="endDate">last day to get prices data</param>
        /// <returns>Daily prices data in OHLC format</returns>
        private string RequestDailyPrice(string symbol, string startDate, string endDate)
        {
            string query = $"/api/v4/historical-price/{symbol}/1/day/{startDate}/{endDate}?apikey={_fmpApiKey}";
            string url = _fmpDomain + query;
            var response = _httpClient.GetStringAsync(url).GetAwaiter().GetResult();
            return response;
        }
    }
}