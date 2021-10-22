﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Database.Models;
using Crunch.Strategies.Overnight.Reports;

namespace Crunch.Strategies.Overnight
{
    public class OvernightStats
    {
        public List<SingleSymbolStats> Stats { get; }

        public OvernightStats(List<SingleSymbolStats> stats)
        {
            Stats = stats;
        }

        


    }
}
