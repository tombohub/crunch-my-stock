using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Strategies;

namespace Crunch.Strategies.Crametorium
{
    class CrametoriumStrategy :IStrategy
    {
        public string Name { get; } = "Crametorium Strategy";

        public void PrintStrategyName()
        {
            Console.WriteLine(Name);
        }
    }
}
