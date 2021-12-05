using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Strategies;

namespace Crunch.Strategies.Crametorium
{
    class CrametoriumStrategy : IStrategy
    {
        public string Name { get; } = "Crametorium Strategy";

        public void RunAnalytics(DateOnly date)
        {
            Console.WriteLine(Name);
        }

        public void CreateDataSource()
        {
            Console.WriteLine("Crametorium data source");
        }
    }
}
