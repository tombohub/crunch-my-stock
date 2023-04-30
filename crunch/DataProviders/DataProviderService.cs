using Crunch.Core;
using Crunch.DataProviders.Alphavantage;
using Crunch.DataProviders.Fmp;
using System;
using System.Collections.Generic;

namespace Crunch.DataProviders
{
    public class DataProviderService
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
        /// Get security price data for the trading day
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradingDay"></param>
        /// <returns></returns>
        public SecurityPrice GetSecurityPrice(Security security, TradingDay tradingDay)
        {
            var response = _fmpProvider.GetSecurityDailyPrice(security.Symbol.Value, tradingDay.Date);
            if (response.TotalResults > 1)
            {
                throw new Exception("There's more than one result from security daily price request");
            }
            return new SecurityPrice
            {
                Symbol = security.Symbol,
                SecurityType = security.Type,
                TradingDay = tradingDay,
                OHLC = new OHLC(
                    response.Results[0].Open,
                    response.Results[0].High,
                    response.Results[0].Low,
                    response.Results[0].Close
                               ),
                Volume = response.Results[0].Volume,
            };
        }


        /// <summary>
        /// Get security price data for the given period
        /// </summary>
        /// <param name="security"></param>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public List<SecurityPrice> GetSecurityPrices(Security security, TimePeriod timePeriod)
        {
            var response = _fmpProvider.GetSecurityDailyPrice(security.Symbol.Value, timePeriod.Start, timePeriod.End);

            var securityPrices = new List<SecurityPrice>();
            foreach (var price in response.Results)
            {
                TradingDay tradingDay = new TradingDay(DateOnly.Parse(price.Timestamp));

                SecurityPrice securityPrice = new SecurityPrice
                {
                    Symbol = security.Symbol,
                    SecurityType = security.Type,
                    TradingDay = tradingDay,
                    Volume = price.Volume,
                    OHLC = new OHLC
                    (
                        open: price.Open,
                        high: price.High,
                        low: price.Low,
                        close: price.Close
                    )
                };
                securityPrices.Add(securityPrice);
            }

            return securityPrices;
        }
    }
}