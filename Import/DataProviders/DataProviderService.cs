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
        public List<SecurityDTO> GetSecurities()
        {
            var listedSecurities = _alphavantageProvider.ListingStatusActive();
            var delistedSecurities = _alphavantageProvider.ListingStatusDelisted();

            var securities = new List<SecurityDTO>();

            foreach (var security in listedSecurities)
            {
                securities.Add(new SecurityDTO
                {
                    Symbol = security.Symbol,
                    Type = _alphavantageProvider.MapSecurityType(security.AssetType),
                    Exchange = _alphavantageProvider.MapExchange(security.Exchange),
                    Status = SecurityStatus.Active,
                    IpoDate = security.IpoDate,
                });
            }

            foreach (var security in delistedSecurities)
            {
                securities.Add(new SecurityDTO
                {
                    Symbol = security.Symbol,
                    Type = _alphavantageProvider.MapSecurityType(security.AssetType),
                    Exchange = _alphavantageProvider.MapExchange(security.Exchange),
                    Status = SecurityStatus.Delisted,
                    IpoDate = security.IpoDate,
                    DelistingDate = security.DelistingDate
                });
            }
            return securities;
        }

        /// <summary>
        /// Get security price data for the day
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradingDay"></param>
        /// <returns></returns>
        public async Task<SecurityPrice> GetDailyPriceAsync(Symbol symbol, TradingDay tradingDay)
        {
            return await _fmpProvider.GetSecurityDailyPriceAsync(symbol, tradingDay);
        }
    }
}