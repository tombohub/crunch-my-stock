using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Database.Models
{
    public partial class Schedule
    {
        public long? Id { get; set; }
        public long? WeekNum { get; set; }
        public DateTime? OvernightWeekStart { get; set; }
        public DateTime? OvernightWeekEnd { get; set; }
        public bool? StocksAreDone { get; set; }
        public bool? EtfsAreDone { get; set; }
    }
}
