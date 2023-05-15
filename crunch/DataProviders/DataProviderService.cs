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
        /// Get security price data for between the start date inclusive and end date inclusive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tradingDay"></param>
        /// <returns></returns>
        public List<DataProviderDailyPriceDataDTO> GetSecurityPrice(Security security, TradingDay start, TradingDay end)
        {
            var response = _fmpProvider.GetSecurityDailyPrice(security.Symbol.Value, start.Date, end.Date);

            var secPriceDtos = new List<DataProviderDailyPriceDataDTO>();
            foreach (var item in response.Results)
            {
                DateOnly date = DateOnly.ParseExact(item.Timestamp, "yyyy-MM-dd HH:mm:ss");

                var secPriceDto = new DataProviderDailyPriceDataDTO
                {
                    Symbol = response.Symbol,
                    Date = date,
                    Open = item.Open,
                    High = item.High,
                    Low = item.Low,
                    Close = item.Close,
                    Volume = item.Volume,
                };
                secPriceDtos.Add(secPriceDto);
            }

            return secPriceDtos;
        }
    }
}