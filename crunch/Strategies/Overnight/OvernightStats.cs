using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Domain.Strategies;
using Crunch.Database.Models;
using Crunch.Strategies.Overnight.Reports;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Overnight Stats entity. Use Overnight Repository to create.
    /// </summary>
    public class OvernightStats : IStrategyStats
    {
        public List<SingleSymbolStats> Stats { get; }

        public OvernightStats(List<SingleSymbolStats> stats)
        {
            Stats = stats;
        }
    }
}
