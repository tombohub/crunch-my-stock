using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Report counting how many stocks were up overnight (winners) how many down (losers)
    /// </summary>
    public partial class WinnersLosersCount
    {
        public long Id { get; set; }
        public int WinnersCount { get; set; }
        public int LosersCount { get; set; }
        public DateOnly Date { get; set; }
        public string SecurityType { get; set; }
    }
}
