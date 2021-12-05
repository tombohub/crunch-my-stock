using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    /// <summary>
    /// Represents starting and ending date of time range.
    /// Used for filtering data from sources and database.
    /// </summary>
    internal class TimeRange
    {
        /// <summary>
        /// Starting datetime
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Ending datetime, inclusive
        /// </summary>
        public DateTime End { get; set; }

        public TimeRange(DateTime start, DateTime end)
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
