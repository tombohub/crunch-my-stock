using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Crunch.Strategies.Overnight
{
    class PriceDownloadOptions
    {
        public DateTime Start { get; set; }

        public PriceDownloadOptions(int weekNum)
        {
            Start = CalculateStartDate(weekNum);
        }

        /// <summary>
        /// Calculate start date for Weekly Overnight strategy. Takes holidays into account.
        /// </summary>
        /// <param name="weekNum">calendar week number</param>
        /// <returns>start dat</returns>
        private DateTime CalculateStartDate(int weekNum)
        {
            DateTime start = ISOWeek.ToDateTime(2021, weekNum, DayOfWeek.Monday);
            while (!TradingCalendar.IsTradingDay(start))
            {
                start = start.AddDays(1);
            }
            return start;
            
        }
    }
}
