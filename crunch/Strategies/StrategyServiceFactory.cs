using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies
{
    internal class StrategyServiceFactory
    {
        public static IStrategyService CreateService(Strategy strategy)
        {
            return strategy switch
            {
                Strategy.Overnight => new Overnight.OvernightStrategyService(),
                Strategy.Crametorium => new Crametorium.CrametoriumStrategy(),
                _ => throw new ArgumentException(nameof(strategy))
            };
        }
    }
}
