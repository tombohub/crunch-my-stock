using System;

namespace Crunch.Core
{
    /// <summary>
    /// Represents perio between 2 dates.
    /// Used for filtering data from sources and database.
    /// </summary>
    internal class Period
    {
        /// <summary>
        /// Starting date
        /// </summary>
        public DateOnly Start { get; set; }

        /// <summary>
        /// Ending date, inclusive
        /// </summary>
        public DateOnly End { get; set; }

        public Period(DateOnly start, DateOnly end)
        {
            // start must be earlier or the same datetime as end
            if (start <= end)
            {
                Start = start;
                End = end;
            }
            else
            {
                throw new ArgumentException("Starting date cannot be later than ending date");
            }
        }
    }
}