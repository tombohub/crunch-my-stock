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
using FluentAssertions;
using Crunch.Strategies.Overnight.Reports;
using Crunch.Strategies.Overnight;

namespace CrunchTests
{
    [TestClass]
    public class OvernightReportsTests
    {
        public static OvernightStats Stats { get; set; }
        public static ReportsCalculator ReportsCalculator {get; set;}

        [ClassInitialize]
        public static void GetStats(TestContext context)
        {
            var reader = new StreamReader("OvernightStatsWeek37.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var records = csv.GetRecords<SingleSymbolStats>().ToList();
            Stats = new OvernightStats(records);
            ReportsCalculator = new ReportsCalculator(Stats);
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
            var winnerLosersRatio = ReportsCalculator.CalculateWinnersLosersRatio(securityType);
            Assert.AreEqual(expectedRatio, winnerLosersRatio);

        }

        [TestMethod]
        public void GetSpyOvernightRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var spyOvernightRoi = ReportsCalculator.GetSpyOvernightRoi();
            Assert.AreEqual(spyOvernightRoi, -0.0165, delta: 0.0001);
        }

        [TestMethod]
        public void GetSpyBenchmarkRoi_OvernightStatsData_ReturnsCorrectNumber()
        {
            var spyBenchmarkRoi = ReportsCalculator.GetSpyBenchmarkRoi();
            Assert.AreEqual(spyBenchmarkRoi, -0.0260, delta: 0.0001);
        }

        [DataTestMethod]
        [DataRow(SecurityType.Stock, -0.0177)]
        [DataRow(SecurityType.Etf, -0.0161)]
        public void CalculateAverageOvernightRoi_OvernightStatsData_ReturnsCorrectNumber(SecurityType securityType, double expectedAvgRoi)
        {
            var avgRoi = ReportsCalculator.CalculateAverageOvernightRoi(securityType);
            Assert.AreEqual(avgRoi, expectedAvgRoi, delta: 0.0001);
        }


        [TestMethod]
        public void CalculateTop10Stocks_OvernightStatsData_ReturnsEqual()
        {
            var reader = new StreamReader("Top10StocksReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedTop10 = csv.GetRecords<Top10Report>().ToList();
            var top10 = ReportsCalculator.CalculateTop10(SecurityType.Stock);

            top10.Should().Equal(expectedTop10);
        }

       [TestMethod]
       public void CalculateTop10Etfs_OvernightStatsData_ReturnsEqual()
        {
            var reader = new StreamReader("Top10EtfsReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedTop10 = csv.GetRecords<Top10Report>().ToList();
            var top10 = ReportsCalculator.CalculateTop10(SecurityType.Etf);

            top10.Should().Equal(expectedTop10);
        }

        [TestMethod]
        public void CalculateBottom10Stocks_OvernightStatsData_RetunEqual()
        {
            var reader = new StreamReader("Bottom10StocksReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedBottom10 = csv.GetRecords<Bottom10Report>().ToList();
            var bottom10 = ReportsCalculator.CalculateBottom10(SecurityType.Stock);

            bottom10.Should().Equal(expectedBottom10);
        }

        [TestMethod]
        public void CalculateBottom10Etfs_OvernightStatsData_RetunEqual()
        {
            var reader = new StreamReader("Bottom10EtfsReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedBottom10 = csv.GetRecords<Bottom10Report>().ToList();
            var bottom10 = ReportsCalculator.CalculateBottom10(SecurityType.Etf);

            bottom10.Should().Equal(expectedBottom10);
        }
    }
}
