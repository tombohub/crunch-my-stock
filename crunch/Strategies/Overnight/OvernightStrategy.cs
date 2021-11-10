using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Strategies;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Main class for Overnight strategy. Responsible for initializing the strategy specific objects.
    /// </summary>
    class OvernightStrategy : IStrategy
    {

        /// <inheritdoc/>
        public string Name { get; } = "Overnight Strategy";
        public void PrintStrategyName()
        {
            Console.WriteLine(Name);
        }
    }
}
