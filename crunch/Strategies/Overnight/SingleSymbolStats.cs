using Crunch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Stats for single security symbol
    /// </summary>
    public record SingleSymbolStats
    {
        /// <summary>
        /// Symbol name, ex: MSFT
        /// </summary>
        public string Symbol { get; init; }

        /// <summary>
        /// Security type
        /// </summary>
        public SecurityType SecurityType { get; init; }

        /// <summary>
        /// Roi of the strategy
        /// </summary>
        public double OvernightRoi { get; init; }

        /// <summary>
        /// Roi if the benchmark (buy and hold)
        /// </summary>
        public double BenchmarkRoi { get; init; }
    }
}
