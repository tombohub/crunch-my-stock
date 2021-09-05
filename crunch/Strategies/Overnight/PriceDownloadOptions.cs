using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Crunch.Core;

namespace Crunch.Strategies.Overnight
{
    class PriceDownloadOptions
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public PriceInterval Interval { get; }

        public PriceDownloadOptions(int weekNum)
        {
            Start = CalculateStartDate(weekNum);
            End = CalculateEndDate(weekNum);
            Interval = PriceInterval.OneDay;
        }

        /// <summary>
        /// Calculate start date for Weekly Overnight strategy. Takes holidays into account.
        /// </summary>
        /// <param name="weekNum">calendar week number</param>
        /// <returns>start date</returns>
        private static DateTime CalculateStartDate(int weekNum)
        {
            DateTime start = ISOWeek.ToDateTime(2021, weekNum, DayOfWeek.Monday);
            while (!TradingCalendar.IsTradingDay(start))
            {
                start = start.AddDays(1);
            }
            return start;
        }

        /// <summary>
        /// Calculate end date for Weekly Overnight strategy. Takes holidays into account.
        /// </summary>
        /// <param name="weekNum">calendar week number</param>
        /// <returns>end date</returns>
        private static DateTime CalculateEndDate(int weekNum)
        {
            int nextWeekNum = weekNum + 1;
            DateTime end = ISOWeek.ToDateTime(2021, nextWeekNum, DayOfWeek.Monday);
            while (!TradingCalendar.IsTradingDay(end))
            {
                end = end.AddDays(1);
            }
            return end;
        }

    }
}
