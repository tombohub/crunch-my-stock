using System.Collections.Generic;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Overnight Stats entity. Use Overnight Repository to create.
    /// </summary>
    public class OvernightStats
    {
        public List<SingleSymbolStats> Stats { get; }

        public OvernightStats(List<SingleSymbolStats> stats)
        {
            Stats = stats;
        }
    }
}
