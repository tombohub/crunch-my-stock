using Crunch.Strategies.Overnight.Reports;
using System.Collections.Generic;

namespace Crunch.Strategies.Overnight
{
    public class ReportsCollection
    {
        public WinnersLosersRatioReport WinnersLosersRatio { get; init; }
        public List<SingleSymbolStats> Top10 { get; init; }
        public List<SingleSymbolStats> Bottom10 { get; init; }
        public double AverageOvernightRoi { get; init; }
        public double AverageBenchmarkRoi { get; init; }
        public double SpyOvernightRoi { get; init; }
        public double SpyBenchmarkRoi { get; init; }

    }

}
