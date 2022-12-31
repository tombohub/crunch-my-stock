using System;
using System.Linq;

namespace Crunch.Domain
{
    public record TradingDay
    {
        /// <summary>
        /// Holiday dates in 2021 when market is closed
        /// </summary>
        private readonly DateOnly[] _holidays =
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
            new DateOnly(2021, 12, 24),

            // Marthing Luther Jr. day
            new DateOnly(2022, 1,17),

            // Washington's birthday
            new DateOnly(2022, 2, 21),

            // Good Friday
            new DateOnly(2022, 4, 15),

            // Memorial day
            new DateOnly(2022, 5, 30),

            // Juneteenth National Independence Day
            new DateOnly(2022, 6, 20),

            // Independence day
            new DateOnly(2022, 7, 4),

            // labor day
            new DateOnly(2022, 9, 5),

            // thanks giving day
            new DateOnly(2022, 11, 24),

            // Christmass
            new DateOnly(2022, 12,26)
        };
        public DateOnly Date { get; init; }
        public TradingDay(DateOnly date)
        {
            CheckIfTradingDay(date);
            Date = date;
        }

        /// <summary>
        /// Check if stock market is open on the given date
        /// </summary>
        /// <param name="date">Date to check
        /// <returns></returns>
        private void CheckIfTradingDay(DateOnly date)
        {
            if (
                (date.DayOfWeek == DayOfWeek.Saturday) ||
                (date.DayOfWeek == DayOfWeek.Sunday) ||
                _holidays.Contains(date)
               )
            {
                throw new ArgumentException($"Date {date} is not a trading day.", nameof(date));
            }
        }
    }
}