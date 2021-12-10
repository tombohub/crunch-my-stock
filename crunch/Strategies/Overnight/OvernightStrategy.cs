using Crunch.Domain.Strategies;
using System;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Main class for Overnight strategy. Responsible for initializing the strategy specific objects.
    /// </summary>
    class OvernightStrategy : IStrategy
    {

        /// <inheritdoc/>
        public string Name { get; } = "Overnight Strategy";
        public void RunAnalytics(DateOnly date)
        {
            Console.WriteLine($"Running analytics for overnight strategy for date {date}");

        }
    }
}
