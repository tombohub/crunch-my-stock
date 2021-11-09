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
        private string _name = "Crametorium Strategy";
        public string Name { get { return _name; } }

        public void PrintStrategyName()
        {
            Console.WriteLine(Name);
        }
    }
}
