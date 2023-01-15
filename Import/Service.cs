using Crunch.Database;
using Crunch.Domain;
using CrunchImport.DataProviders;

namespace CrunchImport
{
    internal static class Service
    {
        private static DataProviderService _dataProvider = new DataProviderService();

        /// <summary>
        /// Import today's prices for all securities into database
        /// </summary>
        public static void ImportTodaysPrices()
        {
            var currentDateTime = DateTime.Now;
            Console.WriteLine($"Current date and time is: {currentDateTime}");

            // make sure the trading day is done
            if (currentDateTime.Hour < 16)
            {
                throw new InvalidOperationException("Trading day is not over yet. Run after 16:00");
            }

            // trading day value object throws exception if not trading day
            DateOnly date = DateOnly.FromDateTime(currentDateTime);
            var tradingDay = new TradingDay(date);

            // get symbols from database
            List<Symbol> symbols = Helpers.GetSecuritySymbols();

            // loop over each symbol, get price for the day and save into database.
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Console.WriteLine($"Importing prices for {symbol}...");
                var thread = new Thread(() => ImportSymbolPrice(symbol, tradingDay));
                thread.Start();
                Thread.Sleep(300);
                Console.WriteLine("Prices imported.");
            }
        }

        /// <summary>
        /// Imports price for a symbol from a data source into the database.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="date">Price date</param>
        private static void ImportSymbolPrice(Symbol symbol, TradingDay tradingDay)
        {
            SecurityPrice symbolPrice = _dataProvider.GetDailyPrice(symbol, tradingDay);

            Helpers.SaveDailyPrice(symbolPrice);
        }

        public static void UpdateSecurities()
        {
            Console.WriteLine("Updating securities...");

            List<SecurityDTO> securitiesDto = _dataProvider.GetSecurities();

            // map dto to entity
            var securities = new List<Security>();
            foreach (var security in securitiesDto)
            {
                try
                {
                    securities.Add(new Security
                    {
                        Symbol = new Symbol(security.Symbol),
                        Exchange = security.Exchange,
                        Status = security.Status,
                        Type = security.Type,
                        IpoDate = security.IpoDate,
                        DelistingDate = security.DelistingDate
                    });
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }

            // import to database
            foreach (var security in securities)
            {
                Console.WriteLine($"Updating {security.Symbol.Value}...");
                Helpers.SaveSecurity(security);
                Console.WriteLine($"{security.Symbol.Value} updated.");
            }
        }
    }
}