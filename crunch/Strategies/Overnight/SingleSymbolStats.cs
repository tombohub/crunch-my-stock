using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    public record SingleSymbolStats
    {
        public string Symbol { get; init; }
        public string SecurityType { get; init; }
        public double Roi { get; init; }
        public string Strategy { get; init; }
    }
}
