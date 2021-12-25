using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Report counting how many stocks were up overnight (winners) how many down (losers)
    /// </summary>
    public partial class WinnersLosersCount
    {
        public DateOnly? Date { get; set; }
        public long? WinnersCount { get; set; }
        public long? LosersCount { get; set; }
    }
}
