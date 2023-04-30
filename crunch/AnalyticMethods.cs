using Crunch.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crunch
{
    /// <summary>
    /// Analyize prices data for overnight
    /// </summary>
    internal class AnalyticMethods
    {
        private DailyPricesOvernight _pricesOvernight;

        public AnalyticMethods(DailyPricesOvernight pricesOvernight)
        {
            _pricesOvernight = pricesOvernight;
        }

        /// <summary>
        /// Calculates number of winners and losers for the trading day by security type
        /// </summary>
        /// <returns></returns>
        public List<WinnersLosersCount> WinnersLosers()
        {
            var winnersLosers = new List<WinnersLosersCount>();
            foreach (SecurityType securityType in Enum.GetValues(typeof(SecurityType)))
            {
                var winners = _pricesOvernight.Prices
                    .Where(x => x.SecurityType == securityType)
                    .Where(x => CalculateChangePercent(x.OHLC) > 0)
                    .Count();

                var losers = _pricesOvernight.Prices
                    .Where(x => x.SecurityType == securityType)
                    .Where(x => CalculateChangePercent(x.OHLC) < 0)
                    .Count();

                winnersLosers.Add(new WinnersLosersCount
                {
                    TradingDay = _pricesOvernight.TradingDay,
                    SecurityType = securityType,
                    WinnersCount = winners,
                    LosersCount = losers,
                });
            }

            return winnersLosers;
        }

        /// <summary>
        /// Average roi accross all securities for trading day by security type
        /// </summary>
        /// <returns></returns>
        public List<AverageRoi> AverageRoi()
        {
            var avgRois = new List<AverageRoi>();
            foreach (SecurityType securityType in Enum.GetValues(typeof(SecurityType)))
            {
                decimal averageRoi = _pricesOvernight.Prices
                    .Where(x => x.SecurityType == securityType)
                    .Average(x => CalculateChangePercent(x.OHLC));
                avgRois.Add(new Core.AverageRoi
                {
                    Roi = Math.Round(averageRoi, 2),
                    SecurityType = securityType,
                    TradingDay = _pricesOvernight.TradingDay
                });
            }

            return avgRois;
        }

        /// <summary>
        /// ROI for the SPY
        /// </summary>
        /// <returns></returns>
        public SpyRoi AverageSpyRoi()
        {
            var spyRoi = _pricesOvernight.Prices
                .Where(x => x.Symbol.Value == "SPY")
                .Average(x => CalculateChangePercent(x.OHLC));

            return new SpyRoi
            {
                TradingDay = _pricesOvernight.TradingDay,
                Roi = Math.Round(spyRoi, 2)
            };
        }

        /// <summary>
        /// Calculates price change between open and close in %
        /// </summary>
        /// <param nameasd="price"></param>
        /// <returns></returns>
        private decimal CalculateChangePercent(OHLC price)
        {
            var changePct = (price.Open - price.Close) / price.Close * 100;
            return changePct;
        }
    }
}