using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight.Plots
{
    /// <summary>
    /// Stup a grid layout for composite plotting.
    /// Composite plotting is drawing single plots into one image - multiplot.
    /// </summary>
    class MultiplotLayout
    {
        /// <summary>
        /// Numbr of rows and columns
        /// </summary>
        private Tuple<int, int> _gridTemplate;

        /// <summary>
        /// Size of the Multiplot in pixels
        /// </summary>
        private Tuple<int, int> Size { get; set; }
    }
}
