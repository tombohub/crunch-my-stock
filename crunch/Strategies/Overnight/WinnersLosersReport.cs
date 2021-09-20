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
    class WinnersLosersReport
    {
        // HACK: type can only be 'winners' or 'losers'
        public string Type { get; init; }
        public int Count { get; init; }
    }
}
