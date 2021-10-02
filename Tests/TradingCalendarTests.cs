using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Crunch.Strategies.Overnight;
using Crunch.Database;
using Crunch.Database.Models;
using Crunch.Strategies;

namespace CrunchTests
{
    [TestClass]
    public class TradingCalendarTests
    {
        [TestMethod]
        public void IsTradingDay_Saturday_ReturnsFalse()
        {
            var date = new DateTime(2021, 9, 4);
            bool isTradingDay = TradingCalendar.IsTradingDay(date);
            Assert.IsFalse(isTradingDay);
        }

        [TestMethod]
        public void IsTradingDay_Sunday_ReturnsFalse()
        {
            var date = new DateTime(2021, 9, 5);
            bool isTradingDay = TradingCalendar.IsTradingDay(date);
            Assert.IsFalse(isTradingDay);
        }

        [TestMethod]
        public void IsTradingDay_Holiday_ReturnsFalse()
        {
            var date = new DateTime(2021, 9, 6);
            bool isTradingDay = TradingCalendar.IsTradingDay(date);
            Assert.IsFalse(isTradingDay);
        }

        [TestMethod]
        public void IsTradingDay_Workday_ReturnsTrue()
        {
            var date = new DateTime(2021, 9, 7);
            bool isTradingDay = TradingCalendar.IsTradingDay(date);
            Assert.IsTrue(isTradingDay);
        }
    }


}