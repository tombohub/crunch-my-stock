using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Core;

namespace CrunchTests
{
    [TestClass]
    public class TradingDayTests
    {
        [TestMethod]
        public void IsTradingDayConstruction_NoException()
        {
            var date = new DateOnly(2023, 1, 23);
            var tradingDay = new TradingDay(date);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotTradingDayContruction_ThrowsException()
        {
            var date = new DateOnly(2023, 1, 22);
            var tradingDay = new TradingDay(date);
        }

        [TestMethod]
        public void FindPreviousTradingDay_ReturnsAreEqual()
        {
            var date = new DateOnly(2023, 1, 23);
            var prevTradingDay = new DateOnly(2023, 1, 20);
            var tradingDay = new TradingDay(date);
            Assert.AreEqual(prevTradingDay, tradingDay.FindPreviousTradingDay().Date);
        }

        [TestMethod]
        public void FindPreviousTradingDay_ReturnsAreNotEqual()
        {
            var date = new DateOnly(2023, 1, 23);
            var prevTradingDay = new DateOnly(2023, 1, 21);
            var tradingDay = new TradingDay(date);
            Assert.AreNotEqual(prevTradingDay, tradingDay.FindPreviousTradingDay().Date);
        }

        [TestMethod]
        public void IsPreviousTradingDayDate_ReturnsTrue()
        {
            var date = new DateOnly(2023, 1, 23);
            var prevTradingDay = new DateOnly(2023, 1, 20);

            var tradindDay = new TradingDay(date);

            Assert.IsTrue(tradindDay.IsPreviousTradingDay(prevTradingDay));
        }

        [TestMethod]
        public void IsPreviousTradingDayDate_ReturnsFalse()
        {
            var date = new DateOnly(2023, 1, 23);
            var prevTradingDay = new DateOnly(2023, 1, 21);

            var tradindDay = new TradingDay(date);

            Assert.IsFalse(tradindDay.IsPreviousTradingDay(prevTradingDay));
        }
    }
}