using System.Collections.Generic;
using System.Linq;
using Crunch.Core;

namespace Crunch
{
    /// <summary>
    /// Analyize prices data for overnight
    /// </summary>
    internal class AnalyticMethods
    {
        public WinnersLosersCount WinnersLosers(List<SecurityPriceOvernight> prices, SecurityType securityType)
        {
            var winners = prices
                .Where(x => x.SecurityType == securityType)
                .Where(x => CalculateChangePercent(x.OHLC) > 0)
                .Count();

            var losers = prices
                .Where(x => x.SecurityType == securityType)
                .Where(x => CalculateChangePercent(x.OHLC) < 0)
                .Count();

            var tradingDay = prices[0].TradingDay;
            return new WinnersLosersCount
            {
                TradingDay = tradingDay,
                SecurityType = securityType,
                WinnersCount = winners,
                LosersCount = losers,
            };
        }

        /// <summary>
        /// Average roi accross all securities for overnight
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public decimal AverageRoi(List<SecurityPriceOvernight> prices)
        {
            var avgRoi = prices
                .Average(x => CalculateChangePercent(x.OHLC));

            return avgRoi;
        }

        /// <summary>
        /// ROI for the SPY
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public decimal AverageSpyRoi(List<SecurityPriceOvernight> prices)
        {
            var avgSpyRoi = prices
                .Where(x => x.Symbol.Value == "SPY").ToList()
                .Average(x => CalculateChangePercent(x.OHLC));

            return avgSpyRoi;
        }

        private decimal CalculateChangePercent(OHLC price)
        {
            var changePct = (price.Open - price.Close) / price.Close * 100;
            return changePct;
        }
    }
}