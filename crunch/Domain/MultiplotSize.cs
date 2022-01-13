using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    /// <summary>
    /// Size of multiplot with dimensions in pixels
    /// </summary>
    internal record MultiplotSize
    {
        /// <summary>
        /// Width of multiplot image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of multiplot image
        /// </summary>
        public int Height { get; set; }
    }
}
