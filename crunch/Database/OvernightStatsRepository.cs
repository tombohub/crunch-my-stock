using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            foreach (var stat in statsDb)
            {
                stats.Add(new SingleSymbolStats
                {
                    Roi = stat.ReturnOnInitialCapital,
                    Symbol = stat.Symbol,
                    SecurityType = stat.SecurityType,
                    Strategy = stat.Strategy
                });
            }
            var overnightStats = new OvernightStats(stats);

            return overnightStats;
        }
    }
}
