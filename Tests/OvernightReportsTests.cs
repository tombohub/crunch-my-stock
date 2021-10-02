using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Strategies.Overnight;
using Crunch.Database;
using Crunch.Database.Models;
using Crunch.Domain;


namespace CrunchTests
{
    [TestClass]
    public class OvernightReportsTests
    {
        public static OvernightStats Stats { get; set; }

        [ClassInitialize]
        public static void GetStats(TestContext context)
        {
            var stats = DatabaseAPI.GetWeeklyOvernightStats(37);
            Stats = new OvernightStats(stats);
        }

        [TestMethod]
        public void CalculateWinnersLosersRatio_OvernightStatsData_ReturnsEqual()
        {
            
        }
        [TestMethod]
        public void GetSpyOvernightRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var spyOvernightRoi = Stats.GetSpyOvernightRoi();
            Assert.AreEqual(spyOvernightRoi, -0.016582899999999946);
        }

        [TestMethod]
        public void GetSpyBenchmarkRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var spyBenchmarkRoi = Stats.GetSpyBenchmarkRoi();
            Assert.AreEqual(spyBenchmarkRoi, -0.026090999999999975);
        }

        [DataTestMethod]
        [DataRow(SecurityType.Stock, -0.017692364756611095)]
        [DataRow(SecurityType.Etf, -0.016191809945275748)]
        public void CalculateAverageOvernightRoi_OvernightStocksStatsData_ReturnsCorrectNumber(SecurityType securityType, double result)
        {
            var avgRoi = Stats.CalculateAverageOvernightRoi(securityType);
            Assert.AreEqual(avgRoi, result);
        }

    }
}
