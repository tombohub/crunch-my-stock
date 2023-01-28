using System;
using Crunch.Core;

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
        public static IStrategyService CreateService(StrategyName strategy)
        {
            return strategy switch
            {
                StrategyName.Overnight => new Overnight.OvernightStrategyService(),
                StrategyName.Crametorium => new Crametorium.CrametoriumStrategyService(),
                _ => throw new ArgumentException(nameof(strategy))
            };
        }
    }
}