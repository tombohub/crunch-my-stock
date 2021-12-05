using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies
{
    internal class CliControllersFactory
    {
        public static ICliController CreateCliController(Strategy strategy)
        {
            ICliController controller = strategy switch
            {
                Strategy.Overnight => new Overnight.CliController(),
                _ => throw new ArgumentException("Strategy does not exist", nameof(strategy))
            };
            return controller;
        }
    }
}
