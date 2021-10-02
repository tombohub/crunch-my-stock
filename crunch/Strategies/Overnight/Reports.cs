using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Crunch.Domain;

namespace Crunch.Strategies.Overnight
{
    public class Reports
    {
        public WinnersLosersRatioReport WinnersLosersRatio { get; init; }
        public List<Top10Report> Top10 { get; init; }
        public List<Bottom10Report> Bottom10 { get; init; }
        public double AverageOvernightRoi { get; init; }
        public double AverageBenchmarkRoi { get; init; }
        public double SpyOvernightRoi { get; init; }
        public double SpyBenchmarkRoi { get; init; }
      
    }
       
}
