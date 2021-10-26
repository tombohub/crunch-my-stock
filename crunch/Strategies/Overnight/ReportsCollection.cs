using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Crunch.Domain;
using Crunch.Strategies.Overnight.Reports;

namespace Crunch.Strategies.Overnight
{
    public class ReportsCollection
    {
        public WinnersLosersRatioReport WinnersLosersRatio { get; init; }
        public List<Top10Report> Top10 { get; init; }
        public List<SingleSymbolStats> Bottom10 { get; init; }
        public double AverageOvernightRoi { get; init; }
        public double AverageBenchmarkRoi { get; init; }
        public double SpyOvernightRoi { get; init; }
        public double SpyBenchmarkRoi { get; init; }
      
    }
       
}
