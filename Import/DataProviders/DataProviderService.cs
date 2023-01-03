using Crunch.Domain;
using CrunchImport.DataProviders.Alphavantage;
using CrunchImport.DataProviders.Fmp;

namespace CrunchImport.DataProviders
{
    internal class DataProviderService
    {
        private FmpProvider _fmpProvider = new FmpProvider();
        private AlphavantageProvider _alphavantageProvider = new AlphavantageProvider();

        /// <summary>
        /// Get securities listed on stock exchange
        /// </summary>
        /// <returns></returns>
        public List<Security> GetListedSecurities()
        {
            return _fmpProvider.GetListedSecurities();
            //return _alphavantageProvider.GetListedSecurities();
        }

        /// <summary>
        /// Get security price data for the day
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradingDay"></param>
        /// <returns></returns>
        public SecurityPrice GetDailyPrice(Symbol symbol, TradingDay tradingDay)
        {
            return _fmpProvider.GetSecurityDailyPrice(symbol, tradingDay);
        }
    }
}