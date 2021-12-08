using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    /// <summary>
    /// Represents starting and ending datetime of date range.
    /// Used for filtering data from sources and database.
    /// </summary>
    internal class DateRange
    {
        /// <summary>
        /// Starting datetime
        /// </summary>
        public DateOnly Start { get; set; }

        /// <summary>
        /// Ending datetime, inclusive
        /// </summary>
        public DateOnly End { get; set; }

        public DateRange(DateOnly start, DateOnly end)
        {
            // start must be earlier or the same datetime as end
            if (start <= end)
            {
                Start = start;
                End = end;
            }
            else
            {
                throw new ArgumentException("Starting datetime cannot be later than ending datetime");
            }
        }
    }
}
