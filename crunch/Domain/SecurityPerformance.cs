using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Strategies.Overnight;

namespace Crunch.Domain
{
    /// <summary>
    /// Represents single security DTO for the Bottom 10 Report
    /// </summary>
    internal record SecurityPerformance
    {
        /// <summary>
        /// Security symbol ticker
        /// </summary>
        public string Symbol { get; set; }
        
        /// <summary>
        /// Change in % between starting price and ending price
        /// </summary>
        public double ChangePct { get; set; }
    }
}
