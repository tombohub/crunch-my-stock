using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Strategies.Overnight;
using Crunch.Database.Models;

namespace Crunch.Database
{
    class OvernightStatsRepository: IOvernightStatsRepository
    {
        public OvernightStats GetOvernightStats(int weekNum)
        {
            var db = new stock_analyticsContext();
            List<WeeklyOvernightStat> stats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum).ToList();
            db.Dispose();
            var overnightStats = new OvernightStats(stats);

            return overnightStats;
        }
    }
}
