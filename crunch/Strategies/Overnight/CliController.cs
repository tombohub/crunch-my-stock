using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    internal class CliController : ICliController
    {
        public void RunAnalytics(string date)
        {
            Console.WriteLine("Strategy cli controller running daily analytics for overnight strategy");
        }
    }
}
