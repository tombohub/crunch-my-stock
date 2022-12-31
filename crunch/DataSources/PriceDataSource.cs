using Crunch.DataSources.Fmp.HistoricalPricesEndpoint;
using Crunch.Domain;

namespace Crunch.DataSources
{
    /// <summary>
    /// Responsible for providing stock and etf prices from data source
    /// </summary>
    internal class PriceDataSource
    {
        /// <summary>
        /// Downloads price set for given symbol and interval between start and end date.
        /// Start and end are inclusive.
        /// </summary>
        /// <param name="symbol">Ticker symbol</param>
        /// <param name="timeRange"></param>
        /// <param name="interval">Price interval</param>
        ///
        /// <returns>Price set for the given symbol</returns>
        public DailyPriceSet DownloadData(Symbol symbol, Period timeRange, PriceInterval interval)
        {
            HistoricalPricesRequester requester = new();
            DailyPriceSet priceSet = requester.RequestData(symbol.Value, timeRange, interval);
            return priceSet;
        }
    }
}