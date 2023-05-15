using Crunch;
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
            ImportPrices(date, date);
        }

        public void ImportPrices(DateOnly start, DateOnly end)
        {
            // trading day value object throws exception if not trading day
            var tradingDayStart = new TradingDay(start);
            var tradingDayEnd = new TradingDay(end);

            // get symbols from database
            var db = new DatabaseMethods();
            List<Security> symbols = db.GetSecurities();

            // loop over each symbol, get price for the day and save into database.
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Thread.Sleep(250);
                var thread = new Thread(() => ImportSecurityPrice(symbol, tradingDayStart, tradingDayEnd));
                thread.Start();
            }
        }

        /// <summary>
        /// Imports price for a single security on a given date into the database.
        /// </summary>
        /// <param name="security"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void ImportSecurityPrice(Security security, TradingDay start, TradingDay end)
        {

            Console.WriteLine($"Importing pricesToday for {security.Symbol.Value}...");
            var priceDataDTOs = _dataProvider.GetSecurityPrice(security, start, end);

            var secPrices = priceDataDTOs.Select(x => MapDtoToSecurityPrice(x)).ToList();

            // new instance to work properly in threading. DbContext need
            // new instance for each thread.
            var db = new DatabaseMethods();
            secPrices.ForEach(x => db.SaveDailyPrice(x));
            Console.WriteLine($"Prices imported for {security.Symbol.Value}");
        }

        private void ImportSecurityPrice(Security security, TimePeriod period)
        {

        }

        /// <summary>
        /// Map data provider price data DTO to domain Security Price object
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private SecurityPrice MapDtoToSecurityPrice(DataProviderDailyPriceDataDTO dto)
        {
            var symbol = new Symbol(dto.Symbol);

            var db = new DatabaseMethods();
            var secType = db.GetSymbolSecurityType(symbol);

            var tradingDay = new TradingDay(dto.Date);

            var ohlc = new OHLC(dto.Open, dto.High, dto.Low, dto.Close);

            var volume = dto.Volume;

            var secPrice = new SecurityPrice
            {
                Symbol = symbol,
                SecurityType = secType,
                TradingDay = tradingDay,
                OHLC = ohlc,
                Volume = volume,
            };

            return secPrice;
        }
    }
}
