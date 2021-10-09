using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight.Reports
{
    public record Bottom10Report
    {
        public string Symbol { get; init; }
        public double StrategyRoi { get; init; }
        public double BenchmarkRoi { get; init; }
    }
}
