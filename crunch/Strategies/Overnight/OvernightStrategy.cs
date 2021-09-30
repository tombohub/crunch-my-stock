using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Database.Models;

namespace Crunch.Strategies.Overnight
{
    class OvernightStrategy
    {
        public List<SingleSymbolStats> Stats { get; }

        public OvernightStrategy(List<SingleSymbolStats> stats)
        {
            Stats = stats;
        }

        
    }
}
