using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies
{
    /// <summary>
    /// Command line controller for individual strategy
    /// </summary>
    internal interface ICliController
    {
        /// <summary>
        /// Run 'analyze' command
        /// </summary>
        /// <param name="date"></param>
        public void RunAnalytics(string date);
    }
}
