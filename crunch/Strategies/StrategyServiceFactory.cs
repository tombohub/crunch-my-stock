using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies
{
    internal class StrategyServiceFactory
    {
        /// <summary>
        /// Creates an instance of Strategy service base on selected strategy enum.
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns>Instance of chosen strategy service</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IStrategyService CreateService(Strategy strategy)
        {
            return strategy switch
            {
                Strategy.Overnight => new Overnight.OvernightStrategyService(),
                Strategy.Crametorium => new Crametorium.CrametoriumStrategyService(),
                _ => throw new ArgumentException(nameof(strategy))
            };
        }
    }
}
