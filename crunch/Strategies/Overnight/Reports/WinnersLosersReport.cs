using System;
using Crunch.Domain;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// Represents report for Winners and Losers count
    /// </summary>
    public class WinnersLosersReport
    {
        public DateOnly Date { get; set; }
        public SecurityType SecurityType { get; init; }
        public int WinnersCount { get; init; }
        public int LosersCount { get; init; }
        

    }
}
