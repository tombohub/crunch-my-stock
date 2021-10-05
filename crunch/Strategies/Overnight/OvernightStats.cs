﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Database.Models;

namespace Crunch.Strategies.Overnight
{
    public class OvernightStats
    {
        public List<SingleSymbolStats> Stats { get; }

        public OvernightStats(List<SingleSymbolStats> stats)
        {
            Stats = stats;
        }

        /// <summary>
        /// Calculate the count of winners and losers
        /// </summary>
        /// <returns>Winners and losers count data</returns>
        //TODO: use enum instead of string
        public WinnersLosersRatioReport CalculateWinnersLosersRatio(SecurityType securityType)
        {
            string type = securityType switch
            {
                SecurityType.Stock => "stocks",
                SecurityType.Etf => "etfs",
                _ => throw new NotImplementedException(),
            };

            var winnersCount = Stats.Where(s => s.Roi >= 0)
                .Where(s => s.SecurityType == type)
                .Where(s => s.Strategy == Strategy.Overnight)
                .Count();
            var losersCount = Stats.Where(s => s.Roi < 0)
                .Where(s => s.SecurityType == type)
                .Where(s => s.Strategy == Strategy.Overnight)
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
        /// <returns>Top 10 report data</returns>
        public List<Top10Report> CalculateTop10()
        {
            // sort top 10 from database
            List<SingleSymbolStats> top10 = Stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == Strategy.Overnight)
                .OrderByDescending(s => s.Roi)
                .Take(10)
                .ToList();

            // TODO: decide what object is this one
            var reportData = new List<Top10Report>();
            foreach (var item in top10)
            {
                double benchmarkRoi = Stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == Strategy.Benchmark)
                    .Select(s => s.Roi)
                    .Single();

                reportData.Add(new Top10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.Roi,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }

            return reportData;
        }


        /// <summary>
        /// Calculate bottom 10 securities by Overnight ROI
        /// </summary>
        /// <returns>Report data for each symbol</returns>
        public List<Bottom10Report> CalculateBottom10()
        {
            // sort top 10 from database
            List<SingleSymbolStats> bottom10 = Stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == Strategy.Overnight)
                .OrderBy(s => s.Roi)
                .Take(10)
                .ToList();

            var reportData = new List<Bottom10Report>();
            foreach (var item in bottom10)
            {
                double benchmarkRoi = Stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == Strategy.Benchmark)
                    .Select(s => s.Roi)
                    .Single();

                reportData.Add(new Bottom10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.Roi,
                    BenchmarkRoi = benchmarkRoi
                });

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
            // hack: repeated code
            string type = securityType switch
            {
                SecurityType.Stock => "stocks",
                SecurityType.Etf => "etfs",
                _ => throw new ArgumentException()
            };

            double averageRoi = Stats
                .Where(s => s.Strategy == Strategy.Overnight) // HACK: miagic string
                .Where(s => s.SecurityType == type) 
                .Select(s => s.Roi)
                .Average();

            return averageRoi;
        }

        /// <summary>
        /// Calculate average ROI for benchmark (buy and hold) for selected security type
        /// </summary>
        /// <returns></returns>
        public double CalculateAverageBenchmarkRoi()
        {
            double averageBenchmarkRoi = Stats
                .Where(s => s.Strategy == Strategy.Benchmark) //hack: magic string
                .Where(s => s.SecurityType == "stocks") //hack: magic string
                .Select(s => s.Roi)
                .Average();
            return averageBenchmarkRoi;
        }

        /// <summary>
        /// Get SPY benchmark buy and hold) ROI for the week
        /// </summary>
        /// <returns></returns>
        public double GetSpyBenchmarkRoi()
        {
            double spyRoi = Stats
                .Where(s => s.Symbol == "SPY") //HACK: magic string
                .Where(s => s.Strategy == Strategy.Benchmark) //HACK: magic string
                .Select(s => s.Roi)
                .Single();

            return spyRoi;
        }

        /// <summary>
        /// Select Spy overnight ROI from the stats
        /// </summary>
        /// <returns></returns>
        public double GetSpyOvernightRoi()
        {
            double spyOvernightRoi = Stats
                .Where(s => s.Symbol == "SPY")
                .Where(s => s.Strategy == Strategy.Overnight)
                .Select(s => s.Roi)
                .Single();

            return spyOvernightRoi;
        }

        public Reports CreateReports(SecurityType securityType)
        {
            Reports reports = new Reports
            {
                AverageBenchmarkRoi = CalculateAverageBenchmarkRoi(),
                AverageOvernightRoi = CalculateAverageOvernightRoi(securityType),
                Top10 = CalculateTop10(), //todo: needs security type
                Bottom10 = CalculateBottom10(), //todo: needs security type
                SpyBenchmarkRoi = GetSpyBenchmarkRoi(),
                SpyOvernightRoi = GetSpyOvernightRoi(),
                WinnersLosersRatio = CalculateWinnersLosersRatio(SecurityType.Stock) //todo: deal wih sec type
            };

            return reports;
        }
    }
}
