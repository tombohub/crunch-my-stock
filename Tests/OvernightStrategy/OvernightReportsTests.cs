using Crunch.Domain;
using Crunch.Strategies.Overnight;
using Crunch.Strategies.Overnight.Reports;
using CsvHelper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace CrunchTests.OvernightStrategy
{
    [TestClass]
    public class OvernightReportsTests
    {
        public static OvernightStats Stats { get; set; }
        public static ReportsCalculator ReportsCalculator { get; set; }

        [ClassInitialize]
        public static void GetStats(TestContext context)
        {
            Stats = OvernightStatsCsvRepository.GetOvernightStats();
            ReportsCalculator = new ReportsCalculator(Stats);
        }

        [DataTestMethod]
        [DataRow(1049, 4826, SecurityType.Stock)]
        [DataRow(320, 2019, SecurityType.Etf)]
        public void CalculateWinnersLosersRatio_OvernightStatsData_ReturnsEqual(int winnersCount, int losersCount, SecurityType securityType)
        {
            var expectedRatio = new WinnersLosersReport
            {
                WinnersCount = winnersCount,
                LosersCount = losersCount,
                SecurityType = securityType
            };
            var winnerLosersRatio = ReportsCalculator.CalculateWinnersLosersRatio(securityType);
            Assert.AreEqual(expectedRatio, winnerLosersRatio);

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
            var reader = new StreamReader("OvernightStrategy/CsvData/Top10StocksReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedTop10 = csv.GetRecords<SingleSymbolStats>().ToList();
            var top10 = ReportsCalculator.CalculateTop10(SecurityType.Stock);

            top10.Should().Equal(expectedTop10);
        }

        [TestMethod]
        public void CalculateTop10Etfs_OvernightStatsData_ReturnsEqual()
        {
            var reader = new StreamReader("OvernightStrategy/CsvData/Top10EtfsReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedTop10 = csv.GetRecords<SingleSymbolStats>().ToList();
            var top10 = ReportsCalculator.CalculateTop10(SecurityType.Etf);

            top10.Should().Equal(expectedTop10);
        }

        [TestMethod]
        public void CalculateBottom10Stocks_OvernightStatsData_RetunEqual()
        {
            var reader = new StreamReader("OvernightStrategy/CsvData/Bottom10StocksReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedBottom10Data = csv.GetRecords<SingleSymbolStats>().ToList();
            var bottom10Data = ReportsCalculator.CalculateBottom10(SecurityType.Stock);

            bottom10Data.Should().Equal(expectedBottom10Data);
        }

        [TestMethod]
        public void CalculateBottom10Etfs_OvernightStatsData_RetunEqual()
        {
            var reader = new StreamReader("OvernightStrategy/CsvData/Bottom10EtfsReportData.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            var expectedBottom10 = csv.GetRecords<SingleSymbolStats>().ToList();
            var bottom10 = ReportsCalculator.CalculateBottom10(SecurityType.Etf);

            bottom10.Should().Equal(expectedBottom10);
        }
    }
}
