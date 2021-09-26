using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.UseCases;
using Crunch.Database.Models;

namespace Crunch.Database
{
    class DatabaseAPI
    {
        public void SaveGroups(string data) { }

        /// <summary>
        /// Get overnight strategy stats for the selected week
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Weekly overnight stats</returns>
        //hack: it returns the database model instead of dto
        public static List<WeeklyOvernightStat> GetWeeklyOvernightStats(int weekNum)
        {
            var db = new stock_analyticsContext();
            List<WeeklyOvernightStat> stats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum).ToList();
            db.Dispose();

            return stats;
        }
    }
}
