using Crunch.Core;
using Crunch.Database;
using Crunch.DataProviders;

namespace ImportPrices
{
    internal class ApplicationService
    {
        private readonly DataProviderService _dataProvider = new DataProviderService();

        /// <summary>
        /// Import today's pricesToday for all securities into database
        /// 
        /// Uses threads
        /// </summary>
        public void ImportPrices(DateOnly date)
        {
            // trading day value object throws exception if not trading day
            var tradingDay = new TradingDay(date);

            // get symbols from database
            var db = new DatabaseMethods();
            List<Security> symbols = db.GetSecurities();

            // loop over each symbol, get price for the day and save into database.
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Thread.Sleep(250);
                var thread = new Thread(() => ImportSecurityPrice(symbol, tradingDay));
                thread.Start();
            }
        }

        public void ImportPrices(DateOnly start, DateOnly end)
        {

        }

        /// <summary>
        /// Imports price for a single security on a given date into the database.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="date">Price date</param>
        private void ImportSecurityPrice(Security security, TradingDay tradingDay)
        {

            Console.WriteLine($"Importing pricesToday for {security.Symbol.Value}...");
            SecurityPrice symbolPrice = _dataProvider.GetSecurityPrice(security, tradingDay);

            // new instance to work properly in threading. DbContext need
            // new instance for each thread.
            var db = new DatabaseMethods();
            db.SaveDailyPrice(symbolPrice);
            Console.WriteLine($"Prices imported for {security.Symbol.Value}");

            //Console.WriteLine($"Error: {e.Message} with symbol {security.Symbol.Value}");

        }

        private void ImportSecurityPrice(Security security, TimePeriod period)
        {

        }
    }
}
