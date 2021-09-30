using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Crunch.Domain;

namespace Crunch.Strategies.Overnight
{
    class Reports
    {
        public WinnersLosersRatioReport WinnersLosersRatio { get; }
        public List<Top10Report> Top10 { get; }
        public List<Bottom10Report> Bottom10 { get; }
        public double AverageOvernightRoi { get; }
        public double AverageBenchmarkRoi { get; }
        public double SpyOvernightRoi { get; }
        public double SpyBenchmarkRoi { get; }

        public Reports() 
        {
        }

        
    }

}
