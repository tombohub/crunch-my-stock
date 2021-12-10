using System;
using System.Linq;

namespace Crunch.Domain
{
    internal class CalendarDay
    {
        /// <summary>
        /// Holiday dates in 2021 when market is closed
        /// </summary>
        private static readonly DateOnly[] _holidays =
        {
            // memorial day
            new DateOnly(2021, 5, 31),

            // independance day 
            new DateOnly(2021, 7, 5),

            // labor day
            new DateOnly(2021, 9, 6),

            // thanksgiving day
            new DateOnly(2021, 11, 25),

            // christmas day
            new DateOnly(2021, 12, 24)

        };
        public DateOnly Date { get; init; }

        public bool IsTradingDay => CheckIfTradingDay(Date);

        public DateOnly PreviousTradingDay => FindPreviousTradingDay();

        public CalendarDay(DateOnly date)
        {
            Date = date;
        }

        /// <summary>
        /// Check if stock market is open on the given date
        /// </summary>
        /// <param name="date">Date to check
        /// .</param>
        /// <returns></returns>
        private bool CheckIfTradingDay(DateOnly date)
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

        /// <summary>
        /// Find the previous trading day from the current date
        /// </summary>
        /// <returns></returns>
        private DateOnly FindPreviousTradingDay()
        {
            // take current date
            DateOnly prevDay = Date.AddDays(-1);
            while (CheckIfTradingDay(prevDay) == false)
            {
                prevDay = prevDay.AddDays(-1);
            }
            // go back one day
            // check if that day is trading day
            // if not go back one more day
            // check if that one is trading day
            // repeat process until trading day is true
            return prevDay;
        }
    }
}
