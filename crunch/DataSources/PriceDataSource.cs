using Crunch.Domain;
using Crunch.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.DataSources.Fmp.HistoricalPricesEndpoint;
using Crunch.UseCases;
using Crunch.Domain.OhlcPrice;

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
        public PriceSet DownloadData(string symbol, TimeRange timeRange, PriceInterval interval)
        {
            HistoricalPricesRequester requester = new();
            PriceSet priceSet = requester.RequestData(symbol, timeRange, interval);
            return priceSet;
        }
    }
}
