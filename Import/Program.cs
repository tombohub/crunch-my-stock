using Crunch.Database;
using Crunch.Database.Models;
using CrunchImport.FmpDataProvider;
using Dapper;
using System.Net;
using System.Text.Json;

namespace CrunchImport
{
    internal static class Program
    {
        private static string _fmpApiKey = "***REMOVED***";
        private static string _fmpDomain = "https://financialmodelingprep.com/";

        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "prices":
                    ImportPrices();
                    break;
                case "securities":
                    UpdateSecurities();
                    break;
                default:
                    Console.WriteLine("Valid commands are: 'prices', 'securities'");
                    break;
            }



        }

        private static void UpdateSecurities()
        {
            Console.WriteLine("Updating securities...");
            // get list of tradeable symbols from fmp
            string queryTradeableSymbols = $"/api/v3/available-traded/list?apikey={_fmpApiKey}";
            string urlTradeableSymbols = _fmpDomain + queryTradeableSymbols;
            var webClient = new WebClient();
            string responseTradeableSymbols = webClient.DownloadString(urlTradeableSymbols);
            var tradeableSymbolsApi = JsonSerializer.Deserialize<List<TradeableSymbolsJsonModel>>(responseTradeableSymbols);

            // remove the securities with symbols longer than 4 characters as those are derived securities:
            tradeableSymbolsApi = tradeableSymbolsApi.Where(s => s.Symbol.Length < 5)
            // choose only securities from NYSE, AMEX and NASDAQ exchanges:
            .Where(s => new string[] { "AMEX", "NYSE", "NASDAQ" }.Contains(s.ExchangeShortName)).ToList();

            // get list of all securities because that's where we have the type of security stock or etf
            string queryAllSymbols = $"/api/v3/stock/list?apikey={_fmpApiKey}";
            string urlAllSymbols = _fmpDomain + queryAllSymbols;
            string responseAllSymbols = webClient.DownloadString(urlAllSymbols);
            var allSymbolsApi = JsonSerializer.Deserialize<List<SymbolsListJsonModel>>(responseAllSymbols);

            // join those 2 lists so we can have tradeable symbols with type column
            var securities = tradeableSymbolsApi
                .Join(allSymbolsApi, a => a.Symbol, b => b.Symbol,
                    (a, b) => new SecurityDTO(a.Symbol, b.Type, a.ExchangeShortName))
                .ToList();

            // import to database
            foreach (var security in securities)
            {
                string sql = $@"INSERT INTO public.securities (symbol, type, exchange, updated_at) 
                            VALUES('{security.Symbol}', '{security.Type.Capitalize()}', '{security.Exchange}', '{DateTime.Now}')
                            ON CONFLICT ON CONSTRAINT securities_symbol_un
                            DO UPDATE SET type = '{security.Type.Capitalize()}',
                                           exchange = '{security.Exchange}',
                                           updated_at = '{DateTime.Now}'";
                using var conn = DbConnections.CreatePsqlConnection();
                conn.Execute(sql);
            }

        }

        private static void ImportPrices()
        {
            // set today's date. If it's before 16:00 it's not valid because 
            // prices close at 16:00
            var currentDateTime = DateTime.Now;
            Console.WriteLine($"Current date and time is: {currentDateTime}");

            DateOnly date = DateOnly.FromDateTime(currentDateTime);

            // get symbols from database
            List<string> symbols = GetSymbols();

            // loop over each symbol, get price for the day and save into database. 
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Console.WriteLine($"Importing prices for {symbol}...");
                var thread = new Thread(() => ImportSymbolPrice(symbol, date));
                thread.Start();
                Thread.Sleep(300);
                Console.WriteLine("Prices imported.");
            }
        }

        /// <summary>
        /// Capitalize first letter in a string
        /// </summary>
        /// <param name="str">string to capitalize</param>
        /// <returns>Capitalized string</returns>
        private static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Imports price for a symbol from a data source into the database.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="date">Price date</param>
        private static void ImportSymbolPrice(string symbol, DateOnly date)
        {
            string priceApiResponse;
            priceApiResponse = RequestDailyPrice(symbol, date.ToString("yyyy-MM-dd"));
            var prices = JsonSerializer.Deserialize<SymbolPricesJsonResponse>(priceApiResponse);
            foreach (var item in prices.Results)
            {
                var price = new DailyPriceDTO
                {
                    Date = DateOnly.ParseExact(item.Timestamp, "yyyy-MM-dd HH:mm:ss"),
                    Symbol = prices.Symbol,
                    Open = item.Open,
                    High = item.High,
                    Low = item.Low,
                    Close = item.Close,
                    Volume = item.Volume
                };
                SaveDailyPrice(price);
            }
        }


        /// <summary>
        /// Send a request for the daily price of particular security for period between start and end date
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="startDate">first day to get prices data</param>
        /// <param name="endDate">last day to get prices data</param>
        /// <returns>Daily prices data in OHLC format</returns>
        private static string RequestDailyPrice(string symbol, string startDate, string endDate)
        {
            string query = $"/api/v4/historical-price/{symbol}/1/day/{startDate}/{endDate}?apikey={_fmpApiKey}";
            string url = _fmpDomain + query;
            var webClient = new WebClient();
            string response = webClient.DownloadString(url);
            return response;

        }

        /// <summary>
        /// Send a request for symbol prices on particular day
        /// </summary>
        /// <param name="symbol">security symbol on exchange</param>
        /// <param name="date">date to get prices for</param>
        /// <returns>Day price data in OHLC format</returns>
        private static string RequestDailyPrice(string symbol, string date)
        {
            var response = RequestDailyPrice(symbol: symbol, startDate: date, endDate: date);
            return response;
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