using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Name of the strategy used in stats
    /// </summary>
    public enum Strategy
    {
        /// <summary>
        /// Overnight strategy, main strategy
        /// </summary>
        Overnight,

        /// <summary>
        /// Benchmark strategy used to compare the perfomance of overnight strategy.
        /// In this case it's buy and hold for the whole week
        /// </summary>
        Benchmark
    }
}
