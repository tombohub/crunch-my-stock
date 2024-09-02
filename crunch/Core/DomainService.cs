using System;
using System.Linq;

namespace Crunch.Core
{
    public class DomainService
    {
        public DailyPricesOvernight TransformToOvernightPrices(DailyPricesRegular previousDayPrices, DailyPricesRegular todayPrices)
        {
            Validate(previousDayPrices, todayPrices);
            var securityPrices = previousDayPrices.SecurityPrices
                .Join(
                todayPrices.SecurityPrices,
                prev => prev.Symbol.Value,
                today => today.Symbol.Value,
                (prev, today) => new SecurityPriceOvernight
                {
                    TradingDay = today.TradingDay,
                    Symbol = today.Symbol,
                    SecurityType = today.SecurityType,
                    OHLC = new OHLC(open: prev.OHLC.Close, close: today.OHLC.Open)
                }
                     )
                .ToList();

            var prices = new DailyPricesOvernight
            {
                TradingDay = todayPrices.TradingDay,
                PrevTradingDay = previousDayPrices.TradingDay,
                Prices = securityPrices
            };
            return prices;
        }

        private void Validate(DailyPricesRegular prevDayPrices, DailyPricesRegular todayPrices)
        {
            // check trading day and previous trading day
            if (!todayPrices.TradingDay.IsPreviousTradingDay(prevDayPrices.TradingDay.Date))
            {
                throw new ArgumentException($"{prevDayPrices.TradingDay.Date} is not previous trading day for {todayPrices.TradingDay.Date}");
            }
        }
    }
}