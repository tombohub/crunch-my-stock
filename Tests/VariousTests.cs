using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Core;

namespace CrunchTests
{
    [TestClass]
    public class VariousTests
    {
        [TestMethod]
        public void IsOvernightSecurityPrices_ReturnsTrue()
        {
            var prevDayPrices = new SecurityPrice
            {
                Symbol = new Symbol("AAPL"),
                TradingDay = new TradingDay(new DateOnly(2023, 1, 20)),
                OHLC = new OHLC(1, 2),
                Volume = 100000
            };

            var todayPrice = new SecurityPrice
            {
                Symbol = new Symbol("AAPL"),
                TradingDay = new TradingDay(new DateOnly(2023, 1, 23)),
                OHLC = new OHLC(3, 4),
                Volume = 120000
            };

            var priceOvernight = new SecurityPriceOvernight(prevDayPrices, todayPrice);
            Assert.AreEqual(priceOvernight.Symbol, priceOvernight.Symbol);
        }
    }
}