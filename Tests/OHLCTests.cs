using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Core;

namespace CrunchTests
{
    [TestClass]
    public class OHLCTests
    {
        [TestMethod]
        public void OHLCConstruction_NoException()
        {
            var ohlc = new OHLC(open: 100, high: 200, low: 50, close: 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OHLCConstruction_Exception()
        {
            var ohlc = new OHLC(open: 100, high: 200, low: 150, close: 100);
        }
    }
}