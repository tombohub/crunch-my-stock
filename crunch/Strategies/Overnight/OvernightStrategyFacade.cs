using Crunch.Domain.Strategies;
using System;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Overnight strategy facade.
    /// </summary>
    class OvernightStrategyFacade : IStrategyFacade
    {

        public void Plot()
        {
            Console.WriteLine("Plotting Overnight");
        }
    }
}
