using System;

namespace Crunch.Core
{
    /// <summary>
    /// Represents overnight price
    /// </summary>
    public class SecurityPriceOvernight
    {
        public Symbol Symbol { get; init; }
        public SecurityType SecurityType { get; init; }
        public TradingDay TradingDay { get; init; }
        public TradingDay PreviousTradingDay { get; init; }
        public OHLC OHLC { get; init; }
        public uint Volume { get; init; }

        /// <summary>
        /// Construct overnight price for security
        /// </summary>
        /// <param name="prevDayPrice">previous trading day price</param>
        /// <param name="todayPrice">today trading day price</param>
        public SecurityPriceOvernight(SecurityPrice prevDayPrice, SecurityPrice todayPrice)
        {
            Validate(prevDayPrice, todayPrice);
            Symbol = prevDayPrice.Symbol;
            TradingDay = todayPrice.TradingDay;
            PreviousTradingDay = prevDayPrice.TradingDay;
            OHLC = new OHLC(open: prevDayPrice.OHLC.Close, close: todayPrice.OHLC.Open);
            Volume = (uint)prevDayPrice.Volume;
        }

        public SecurityPriceOvernight()
        {
        }

        private void Validate(SecurityPrice prevDayPrice, SecurityPrice todayPrice)
        {
            // check trading day and previous trading day
            if (!todayPrice.TradingDay.IsPreviousTradingDay(prevDayPrice.TradingDay.Date))
            {
                throw new ArgumentException($"{prevDayPrice.TradingDay.Date} is not previous trading day for {todayPrice.TradingDay.Date}");
            }

            // check if symbols are same
            if (prevDayPrice.Symbol.Value != todayPrice.Symbol.Value)
            {
                throw new ArgumentException($"Symbol {prevDayPrice.Symbol.Value} is not same as {todayPrice.Symbol.Value}");
            }
        }
    }
}