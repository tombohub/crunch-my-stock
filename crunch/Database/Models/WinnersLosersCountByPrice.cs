using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Number of winning and losing securities grouped by custom price ranges
    /// </summary>
    public partial class WinnersLosersCountByPrice
    {
        public DateOnly? Date { get; set; }
        public long? WinnersCount { get; set; }
        public long? LosersCount { get; set; }
        public string PriceRange { get; set; }
        public int? GroupOrder { get; set; }
    }
}
