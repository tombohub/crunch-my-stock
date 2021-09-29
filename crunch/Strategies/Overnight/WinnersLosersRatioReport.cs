using Crunch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Report data model for Winners and Losers count
    /// </summary>
    class WinnersLosersRatioReport
    {
        public SecurityType SecurityType { get; init; }
        public int WinnersCount { get; init; }
        public int LosersCount { get; init; }
    }
}
