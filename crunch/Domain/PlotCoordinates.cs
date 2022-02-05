using Crunch.Strategies.Overnight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    /// <summary>
    /// Coordinates and size of individual plot inside the multiplot layout
    /// </summary>
    internal record PlotCoordinates
    {
        public ReportName Report { get; init; }
        
        /// <summary>
        /// X coordinate of the plot
        /// </summary>
        public int X { get; init; }

        /// <summary>
        /// Y coordinate of the plot in multiplot
        /// </summary>
        public int Y { get; init; }

        /// <summary>
        /// Width of the plot in multiplot
        /// </summary>
        public int Width { get; init; }

        /// <summary>
        /// Height of the plot in multiplot
        /// </summary>
        public int Height { get; init; }
    }
}
