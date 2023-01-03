using System.Net;
using System.Text.Json;
using Crunch.Domain;
using CrunchImport.DataProviders.Fmp.Responses;

namespace CrunchImport.DataProviders.Fmp
{
    internal class FmpProvider
    {
        private const string _fmpApiKey = "***REMOVED***";
        private const string _fmpDomain = "https://financialmodelingprep.com/";
        private WebClient _webClient = new WebClient();

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
        /// Get securities listed on stock exchange
        /// </summary>
        /// <returns></returns>
        public List<Security> GetListedSecurities()
        {
            var tradeableSecurities = RequestTradeableSecurities();
            var allAvailableSecurities = RequestAllAvailableSecurities();

            // join those 2 lists so we can have tradable symbols with type column
            var securitiesResponseCombined = tradeableSecurities
                .Join(allAvailableSecurities, a => a.Symbol, b => b.Symbol,
                    (a, b) => new
                    {
                        a.Symbol,
                        b.Type,
                        a.ExchangeShortName,
                    })
                .ToList();

            var securities = new List<Security>();
            foreach (var security in securitiesResponseCombined)
            {
                try
                {
                    securities.Add(new Security
                    {
                        Symbol = new Symbol(security.Symbol),
                        Type = new SecurityType(security.Type),
                        Exchange = new Exchange(security.ExchangeShortName),
                        IsTradable = true
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return securities;
        }

        /// <summary>
        /// Request all available securities, either tradeable or delisted, for which FMP has data
        /// </summary>
        /// <returns></returns>
        private List<SymbolsListJsonModel> RequestAllAvailableSecurities()
        {
            // get list of all securities because that's where we have the type of security stock or etf
            string queryAllSymbols = $"/api/v3/stock/list?apikey={_fmpApiKey}";
            string urlAllSymbols = _fmpDomain + queryAllSymbols;
            string responseAllSymbols = _webClient.DownloadString(urlAllSymbols);
            var allSymbolsApi = JsonSerializer.Deserialize<List<SymbolsListJsonModel>>(responseAllSymbols);
            return allSymbolsApi;
        }

        /// <summary>
        /// Request only securities that are currently active on stock exchange.
        /// </summary>
        /// <returns></returns>
        private List<TradeableSymbolsJsonModel> RequestTradeableSecurities()
        {
            // get list of tradeable symbols from fmp
            string queryTradeableSymbols = $"/api/v3/available-traded/list?apikey={_fmpApiKey}";
            string urlTradeableSymbols = _fmpDomain + queryTradeableSymbols;
            string responseTradeableSymbols = _webClient.DownloadString(urlTradeableSymbols);
            var tradeableSymbolsApi = JsonSerializer.Deserialize<List<TradeableSymbolsJsonModel>>(responseTradeableSymbols);
            return tradeableSymbolsApi;
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
            string response = _webClient.DownloadString(url);
            return response;
        }
    }
}