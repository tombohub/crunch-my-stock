using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crunch.Strategies.Overnight;
using Crunch.Database;
using Crunch.Domain;


namespace CrunchTests
{
    [TestClass]
    public class OvernightReportsTests
    {
        [TestMethod]
        public void GetSpyOvernightRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var stats = DatabaseAPI.GetWeeklyOvernightStats(37);
            var overnightStats = new OvernightStats(stats);
            var spyOvernightRoi = overnightStats.GetSpyOvernightRoi();
            Assert.AreEqual(spyOvernightRoi, -0.016582899999999946);
        }

        [TestMethod]
        public void GetSpyBenchmarkRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var stats = DatabaseAPI.GetWeeklyOvernightStats(37);
            var overnightStats = new OvernightStats(stats);
            var spyBenchmarkRoi = overnightStats.GetSpyBenchmarkRoi();
            Assert.AreEqual(spyBenchmarkRoi, -0.026090999999999975);
        }

        [TestMethod]
        public void CalculateAverageOvernightRoi_OvernightStocksStatsData_ReturnsCorrectNumber()
        {
            var stats = DatabaseAPI.GetWeeklyOvernightStats(37);
            var overnightStats = new OvernightStats(stats);
            var avgRoi = overnightStats.CalculateAverageOvernightRoi(SecurityType.Stock);
            Assert.AreEqual(avgRoi, -0.017692364756611095);
        }

        [TestMethod]
        public void CalculateAverageOvernightRoi_OvernightEtfsStatsData_ReturnsCorrectNumber()
        {
            var stats = DatabaseAPI.GetWeeklyOvernightStats(37);
            var overnightStats = new OvernightStats(stats);
            var avgRoi = overnightStats.CalculateAverageOvernightRoi(SecurityType.Etf);
            Assert.AreEqual(avgRoi, -0.016191809945275748);
        }
    }
}
