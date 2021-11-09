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

        private readonly string _name = "Overnight Strategy";

        /// <inheritdoc/>
        public string Name { get { return _name; } }
        public void PrintStrategyName()
        {
            Console.WriteLine(Name);
        }
    }
}
