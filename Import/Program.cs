using Crunch.Database;
using Crunch.Database.Models;
using Dapper;
using System.Net;
using System.Text.Json;

namespace Import
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // set today's date. If it's before 16:00 it's not valid because 
            // prices close at 16:00
            var today = DateTime.Now;
            Console.WriteLine($"Current dat and time is: {today}");
            if (today.TimeOfDay < new TimeSpan(16, 0, 0))
            {
                throw new InvalidOperationException("Current stock market is still open. Try after 16:00");
            }

            // get symbols from database
            List<string> symbols = GetSymbols();

            // loop over each symbol, get price for the day and save into database. 
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {

                var priceApi = RequestDailyPrice("AAPL", "2022-08-01");
                foreach (var item in priceApi.Results)
                {
                    var price = new DailyPriceDTO
                    {
                        Date = DateOnly.ParseExact(item.Timestamp, "yyyy-MM-dd HH:mm:ss"),
                        Symbol = priceApi.Symbol,
                        Open = item.Open,
                        High = item.High,
                        Low = item.Low,
                        Close = item.Close,
                        Volume = item.Volume
                    };
                    SaveDailyPrice(price);
                }
            }


        }

        /// <summary>
        /// Send a request for the daily price of particular security for period between start and end date
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="startDate">first day to get prices data</param>
        /// <param name="endDate">last day to get prices data</param>
        /// <returns>Daily prices data in OHLC format</returns>
        private static SymbolPricesJsonResponse RequestDailyPrice(string symbol, string startDate, string endDate)
        {
            string apiKey = "***REMOVED***";
            string baseUrl = "https://financialmodelingprep.com/api/v4/";
            string query = $"historical-price/{symbol}/1/day/{startDate}/{endDate}?apikey={apiKey}";
            string url = baseUrl + query;
            var webClient = new WebClient();
            var response = webClient.DownloadString(url);

            var jsonPricesData = JsonSerializer.Deserialize<SymbolPricesJsonResponse>(response);
            return jsonPricesData;
        }

        /// <summary>
        /// Send a request for symbol prices on particular day
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="date">date to get prices for</param>
        /// <returns>Day price data in OHLC format</returns>
        private static SymbolPricesJsonResponse RequestDailyPrice(string symbol, string date)
        {
            var prices = RequestDailyPrice(symbol: symbol, startDate: date, endDate: date);
            return prices;
        }

        /// <summary>
        /// Save daily price data to database. If price for that day/symbol exists 
        /// then it will be updated. 
        /// </summary>
        /// <param name="price"></param>
        private static void SaveDailyPrice(DailyPriceDTO price)
        {
            string sql = @$"insert into 
                 public.prices_daily (date, symbol, open, high, low, close, volume) 
                VALUES ('{price.Date}', '{price.Symbol}', '{price.Open}', '{price.High}', 
                '{price.Low}', '{price.Close}', '{price.Volume}')
                ON CONFLICT ON CONSTRAINT date_symbol_un
                DO UPDATE SET 
                              open = '{price.Open}',
                              high = '{price.High}',
                              low = '{price.Low}',
                              close = '{price.Close}',
                              volume = '{price.Volume}';";

            using var conn = DbConnections.CreatePsqlConnection();
            conn.Execute(sql);

        }

        /// <summary>
        /// Get list of security symbols from database
        /// </summary>
        /// <returns></returns>
        private static List<string> GetSymbols()
        {
            var context = new stock_analyticsContext();
            var symbols = context.Securities.Select(x => x.Symbol).ToList();
            return symbols;
        }



    }
}