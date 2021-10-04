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
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.IO;
using System.Reflection;
using CsvHelper.Configuration;

namespace CrunchTests
{
    [TestClass]
    public class OvernightReportsTests
    {
        public static OvernightStats Stats { get; set; }

        [ClassInitialize]
        public static void GetStats(TestContext context)
        {
            var repo = new OvernightStatsRepository();
            Stats = repo.GetOvernightStats(37);
        }

        [DataTestMethod]
        [DataRow(1049, 4826, SecurityType.Stock)]
        [DataRow(320, 2019, SecurityType.Etf)]
        public void CalculateWinnersLosersRatio_OvernightStatsData_ReturnsEqual(int winnersCount, int losersCount, SecurityType securityType)
        {
            var expectedRatio = new WinnersLosersRatioReport
            {
                WinnersCount = winnersCount,
                LosersCount = losersCount,
                SecurityType = securityType
            };
            var winnerLosersRatio = Stats.CalculateWinnersLosersRatio(securityType);
            Assert.AreEqual(expectedRatio, winnerLosersRatio);

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
        public void CalculateAverageOvernightRoi_OvernightStatsData_ReturnsCorrectNumber(SecurityType securityType, double result)
        {
            var avgRoi = Stats.CalculateAverageOvernightRoi(securityType);
            Assert.AreEqual(avgRoi, result);
        }


        [TestMethod]
        public void CalculateTop10_OvernightStatsData_ReturnsEqual()
        {
            var reader = new StreamReader("Top10StocksReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Top10Report>().ToList();
            var top10 = Stats.CalculateTop10();
            

        }

    }
}
