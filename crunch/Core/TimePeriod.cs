using System;

namespace Crunch.Core
{
    /// <summary>
    /// Time period
    /// </summary>
    public record TimePeriod
    {
        /// <summary>
        /// Period start date
        /// </summary>
        public DateOnly Start { get; }

        /// <summary>
        /// Period end date
        /// </summary>
        public DateOnly End { get; }

        public TimePeriod(DateOnly start, DateOnly end)
        {
            if (start > end)
            {
                throw new ArgumentException("Start date cannot be later than end date");
            }
            Start = start;
            End = end;
        }
    }
}
