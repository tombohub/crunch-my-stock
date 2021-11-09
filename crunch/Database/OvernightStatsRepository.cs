using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Strategies.Overnight;
using Crunch.Database.Models;

namespace Crunch.Database
{
    /// <summary>
    /// Repository for Overnight Stats entity
    /// </summary>
    public class OvernightStatsRepository
    {
        /// <summary>
        /// Get Weekly overnight stats entity for the given week
        /// </summary>
        /// <param name="weekNum"></param>
        /// <returns></returns>
        public OvernightStats GetOvernightStats(int weekNum)
        {
            var db = new stock_analyticsContext();

            List<WeeklyOvernightStat> overnightStats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum)
                .Where(s => s.Strategy == "overnight")
                .ToList();
            List<WeeklyOvernightStat> benchmarkStats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum)
                .Where(s => s.Strategy == "benchmark")
                .ToList();
            var statsDb = overnightStats
                .Join(benchmarkStats, overnight => overnight.Symbol, benchmark => benchmark.Symbol,
                    (overnight, benchmark) => new
                    {
                        Symbol = overnight.Symbol,
                        SecurityType = overnight.SecurityType,
                        OvernightRoi = overnight.ReturnOnInitialCapital,
                        BenchmarkRoi = benchmark.ReturnOnInitialCapital
                    })
                .ToList();

            db.Dispose();

            var stats = new List<SingleSymbolStats>();
            foreach (var statDb in statsDb)
            {
                SecurityType securityType = statDb.SecurityType switch
                {   // hack: values are from database
                    "stocks" => SecurityType.Stock,
                    "etfs" => SecurityType.Etf,
                    _ => throw new NotImplementedException()
                };
                                
                stats.Add(new SingleSymbolStats
                (
                    Symbol: statDb.Symbol,
                    SecurityType: securityType,
                    OvernightRoi: statDb.OvernightRoi,
                    BenchmarkRoi: statDb.BenchmarkRoi
                ));
            }
            var overnightStatsEntity = new OvernightStats(stats);

            return overnightStatsEntity;
        }
    }
}
