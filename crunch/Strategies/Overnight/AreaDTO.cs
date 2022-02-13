using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Strategies.Overnight.Multiplot;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// DTO for getting area coordinates and name from database.
    /// </summary>
    internal record AreaDTO
    {
        /// <summary>
        /// Name of the report
        /// </summary>
        public AreaName AreaName { get; init; }

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
