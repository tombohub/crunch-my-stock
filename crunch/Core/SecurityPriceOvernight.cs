using System;

namespace Crunch.Core
{
    /// <summary>
    /// Security price for overnight price movement
    /// </summary>
    public class SecurityPriceOvernight
    {
        public Symbol Symbol { get; init; }
        public TradingDay Date { get; init; }
        public TradingDay PreviousTradingDay { get; init; }
        public OHLC Price { get; init; }
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
            Date = todayPrice.Date;
            PreviousTradingDay = prevDayPrice.Date;
            Price = new OHLC(open: prevDayPrice.Price.Close, close: todayPrice.Price.Open);
            Volume = prevDayPrice.Volume;
        }

        private void Validate(SecurityPrice prevDayPrice, SecurityPrice todayPrice)
        {
            // check trading day and previous trading day
            if (!todayPrice.Date.IsPreviousTradingDay(prevDayPrice.Date.Date))
            {
                throw new ArgumentException($"{prevDayPrice.Date.Date} is not previous trading day for {todayPrice.Date.Date}");
            }

            // check if symbols are same
            if (prevDayPrice.Symbol.Value != todayPrice.Symbol.Value)
            {
                throw new ArgumentException($"Symbol {prevDayPrice.Symbol.Value} is not same as {todayPrice.Symbol.Value}");
            }
        }
    }
}