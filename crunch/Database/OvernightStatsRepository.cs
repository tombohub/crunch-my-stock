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
            List<WeeklyOvernightStat> statsDb = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum)
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
                Strategy strategy = statDb.Strategy switch
                {   // hack: values are from database
                    "overnight" => Strategy.Overnight,
                    "benchmark" => Strategy.Benchmark,
                    _ => throw new NotImplementedException()
                };

                stats.Add(new SingleSymbolStats
                {
                    Roi = statDb.ReturnOnInitialCapital,
                    Symbol = statDb.Symbol,
                    SecurityType = securityType,
                    Strategy = strategy
                });
            }
            var overnightStats = new OvernightStats(stats);

            return overnightStats;
        }
    }
}
