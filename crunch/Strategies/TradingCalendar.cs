using System;
using System.Linq;

namespace Crunch.Strategies
{
    public class TradingCalendar
    {
        /// <summary>
        /// Holiday dates in 2021 when market is closed
        /// </summary>
        private static readonly DateTime[] _holidays =
        {
            // memorial day
            new DateTime(2021, 5, 31),

            // independance day 
            new DateTime(2021, 7, 5),

            // labor day
            new DateTime(2021, 9, 6),

            // thanksgiving day
            new DateTime(2021, 11, 25),

            // christmas day
            new DateTime(2021, 12, 24)

        };

        /// <summary>
        /// Check if stock market is open on the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsTradingDay(DateTime date)
        {
            bool isTradingDay;
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
            {
                isTradingDay = false;
            }
            else if (_holidays.Contains(date))
            {
                isTradingDay = false;
            }
            else
            {
                isTradingDay = true;
            }


            return isTradingDay;
        }
    }
}
