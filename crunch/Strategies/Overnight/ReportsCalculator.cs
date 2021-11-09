using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Strategies.Overnight.Reports;

namespace Crunch.Strategies.Overnight
{
    public class ReportsCalculator
    {
        private OvernightStats _stats;
        public ReportsCalculator(OvernightStats stats)
        {
            _stats = stats;
        }

        /// <summary>
        /// Calculate the count of winners and losers
        /// </summary>
        /// <returns>Winners and losers count data</returns>
        public WinnersLosersRatioReport CalculateWinnersLosersRatio(SecurityType securityType)
        {
            var winnersCount = _stats.Stats.Where(s => s.OvernightRoi >= 0)
                .Where(s => s.SecurityType == securityType)
                .Count();
            var losersCount = _stats.Stats.Where(s => s.OvernightRoi < 0)
                .Where(s => s.SecurityType == securityType)
                .Count();

            var reportData = new WinnersLosersRatioReport
            {
                SecurityType = securityType,
                WinnersCount = winnersCount,
                LosersCount = losersCount
            };
            return reportData;
        }

        /// <summary>
        /// Calculate Top 10 securities by ROI for overnight strategy
        /// </summary>
        /// <param name="securityType"></param>
        /// <returns>Top 10 report data</returns>
        public List<Top10Report> CalculateTop10(SecurityType securityType)
        {
            // sort top 10 from database
            List<SingleSymbolStats> top10 = _stats.Stats
                .Where(s => s.SecurityType == securityType)
                .OrderByDescending(s => s.OvernightRoi)
                .Take(10)
                .ToList();

            // TODO: decide what object is this one
            var reportData = new List<Top10Report>();
            foreach (var item in top10)
            {
                double benchmarkRoi = _stats.Stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Select(s => s.BenchmarkRoi)
                    .Single();

                reportData.Add(new Top10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.OvernightRoi,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }

            return reportData;
        }


        /// <summary>
        /// Calculate bottom 10 securities by Overnight ROI
        /// </summary>
        /// <param name="securityType"></param>
        /// <returns>Report data for each symbol</returns>
        public List<SingleSymbolStats> CalculateBottom10(SecurityType securityType)
        {
            // sort bottom 10 from database
            List<SingleSymbolStats> bottom10 = _stats.Stats
                .Where(s => s.SecurityType == securityType)
                .OrderBy(s => s.OvernightRoi)
                .Take(10)
                .ToList();

            var reportData = new List<SingleSymbolStats>();
            foreach (var item in bottom10)
            {
                double benchmarkRoi = _stats.Stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Select(s => s.BenchmarkRoi)
                    .Single();

                reportData.Add(new SingleSymbolStats
                (
                    Symbol: item.Symbol,
                    SecurityType: securityType,
                    OvernightRoi: item.OvernightRoi,
                    BenchmarkRoi: benchmarkRoi
                ));

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }

            return reportData;
        }

        /// <summary>
        /// Calculate Average ROI for selected security type of Overnight strategy
        /// </summary>
        /// <returns>Average ROI</returns>
        public double CalculateAverageOvernightRoi(SecurityType securityType)
        {
            double averageRoi = _stats.Stats
                .Where(s => s.SecurityType == securityType)
                .Select(s => s.OvernightRoi)
                .Average();

            return averageRoi;
        }

        /// <summary>
        /// Calculate average ROI for benchmark (buy and hold) for selected security type
        /// </summary>
        /// <returns></returns>
        public double CalculateAverageBenchmarkRoi()
        {
            double averageBenchmarkRoi = _stats.Stats
                .Where(s => s.SecurityType == SecurityType.Stock)
                .Select(s => s.BenchmarkRoi)
                .Average();
            return averageBenchmarkRoi;
        }

        /// <summary>
        /// Select Spy overnight ROI from the stats
        /// </summary>
        /// <returns></returns>
        public double GetSpyOvernightRoi()
        {
            double spyOvernightRoi = _stats.Stats
                .Where(s => s.Symbol == "SPY")
                .Select(s => s.OvernightRoi)
                .Single();

            return spyOvernightRoi;
        }
        
        /// <summary>
        /// Get SPY benchmark buy and hold) ROI for the week
        /// </summary>
        /// <returns></returns>
        public double GetSpyBenchmarkRoi()
        {
            double spyRoi = _stats.Stats
                .Where(s => s.Symbol == "SPY") //HACK: magic string
                .Select(s => s.BenchmarkRoi)
                .Single();

            return spyRoi;
        }

        public ReportsCollection CreateReports(SecurityType securityType)
        {
            ReportsCollection reports = new ReportsCollection
            {
                AverageBenchmarkRoi = CalculateAverageBenchmarkRoi(),
                AverageOvernightRoi = CalculateAverageOvernightRoi(securityType),
                Top10 = CalculateTop10(securityType),
                Bottom10 = CalculateBottom10(securityType),
                SpyBenchmarkRoi = GetSpyBenchmarkRoi(),
                SpyOvernightRoi = GetSpyOvernightRoi(),
                WinnersLosersRatio = CalculateWinnersLosersRatio(securityType)
            };

            return reports;
        }
    }
}
