﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Tests
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