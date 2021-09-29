using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Crunch.Domain;

namespace Crunch.Strategies.Overnight
{
    class Reports
    {
        public WinnersLosersRatioReport WinnersLosersRatio { get; }
        public Reports(List<WeeklyOvernightStat> stats, SecurityType securityType) // TODO: use enum instead of string
        {
            WinnersLosersRatio = CalculateWinnersLosersRatio(stats, securityType);
        }
        /// <summary>
        /// Calculate the count of winners and losers
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Winners and losers count data</returns>
        //TODO: use enum instead of string
        public static WinnersLosersRatioReport CalculateWinnersLosersRatio(List<WeeklyOvernightStat> stats, SecurityType securityType)
        {
            string type = securityType switch
            {
                SecurityType.Stock => "stocks",
                SecurityType.Etf => "etf",
                _ => throw new NotImplementedException(),
            };

            var winnersCount = stats.Where(s => s.ReturnOnInitialCapital >= 0)
                .Where(s => s.SecurityType == type)
                .Where(s => s.Strategy == "overnight")
                .Count();
            var losersCount = stats.Where(s => s.ReturnOnInitialCapital < 0)
                .Where(s => s.SecurityType == type)
                .Where(s => s.Strategy == "overnight")
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
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Top 10 report data</returns>
        public static List<Top10Report> CalculateTop10(List<WeeklyOvernightStat> stats)
        {
            // sort top 10 from database
            List<WeeklyOvernightStat> top10 = stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == "overnight")
                .OrderByDescending(s => s.ReturnOnInitialCapital)
                .Take(10)
                .ToList();

            // TODO: decide what object is this one
            var reportData = new List<Top10Report>();
            foreach (var item in top10)
            {
                double benchmarkRoi = stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == "benchmark")
                    .Select(s => s.ReturnOnInitialCapital)
                    .Single();

                reportData.Add(new Top10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.ReturnOnInitialCapital,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }

            return reportData;
        }


        /// <summary>
        /// Calculate bottom 10 securities by Overnight ROI
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Report data for each symbol</returns>
        public static List<Bottom10Report> CalculateBottom10(List<WeeklyOvernightStat> stats)
        {
            // sort top 10 from database
            List<WeeklyOvernightStat> bottom10 = stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == "overnight")
                .OrderBy(s => s.ReturnOnInitialCapital)
                .Take(10)
                .ToList();

            var reportData = new List<Bottom10Report>();
            foreach (var item in bottom10)
            {
                double benchmarkRoi = stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == "benchmark")
                    .Select(s => s.ReturnOnInitialCapital)
                    .Single();

                reportData.Add(new Bottom10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.ReturnOnInitialCapital,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }
            return reportData;
        }

        /// <summary>
        /// Calculate Average ROI for selected security type of Overnight strategy
        /// </summary>
        /// <param name="stats">Overnight stats for the week</param>
        /// <returns>Average ROI</returns>
        public static double CalculateAverageOvernightRoi(List<WeeklyOvernightStat> stats)
        {
            double averageRoi = stats
                .Where(s => s.Strategy == "overnight") // HACK: miagic string
                .Where(s => s.SecurityType == "stocks") //hack: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Average();

            return averageRoi;
        }

        /// <summary>
        /// Calculate average ROI for benchmark (buy and hold) for selected security type
        /// </summary>
        /// <param name="stats">Weekly overnight stats for the week</param>
        /// <returns></returns>
        public static double CalculateAverageBenchmarkRoi(List<WeeklyOvernightStat> stats)
        {
            double averageBenchmarkRoi = stats
                .Where(s => s.Strategy == "benchmark") //hack: magic string
                .Where(s => s.SecurityType == "stocks") //hack: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Average();
            return averageBenchmarkRoi;
        }

        /// <summary>
        /// Get SPY benchmark buy and hold) ROI for the week
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public static double GetSpyBenchmarkRoi(List<WeeklyOvernightStat> stats)
        {
            double spyRoi = stats
                .Where(s => s.Symbol == "SPY") //HACK: magic string
                .Where(s => s.Strategy == "benchmark") //HACK: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Single();

            return spyRoi;
        }

        public static double GetSpyOvernightRoi(List<WeeklyOvernightStat> stats)
        {
            double spyOvernightRoi = stats
                .Where(s => s.Symbol == "SPY")
                .Where(s => s.Strategy == "overnight")
                .Select(s => s.ReturnOnInitialCapital)
                .Single();

            return spyOvernightRoi;
        }
    }

}
