using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight.Plots
{
    /// <summary>
    /// Setup a grid layout for composite plotting.
    /// Composite plotting is drawing single plots into one image - multiplot.
    /// </summary>
    class MultiplotLayout
    {
        /// <summary>
        /// Height of the multiplot in pixels
        /// </summary>
        private int _height;

        /// <summary>
        /// Width of multiplot in pixels
        /// </summary>
        private int _width;
        
        /// <summary>
        /// Number of rows in grid
        /// </summary>
        private int _rowsNumber;

        /// <summary>
        /// Number of columns in grid
        /// </summary>
        private int _columnsNumber;

        /// <summary>
        /// Initialize multiplot layout grid object
        /// </summary>
        /// <param name="width">Width of multiplot in pixels</param>
        /// <param name="height">Height of multiplot in pixels</param>
        /// <param name="rowsNumber">Number of rows in grid</param>
        /// <param name="columnsNumber">Number of columns in grid</param>
        public MultiplotLayout(int width, int height, int rowsNumber, int columnsNumber)
        {
            _width = width;
            _height = height;
            _rowsNumber = rowsNumber;
            _columnsNumber = columnsNumber;
        }

        /// <summary>
        /// Adds plot to the multiplot detrmined by starting grid point and ending grid point
        /// </summary>
        /// <param name="plot">Plot to </param>
        /// <param name="columnStart"></param>
        /// <param name="rowStart"></param>
        /// <param name="columnEnd"></param>
        /// <param name="rowEnd"></param>
        public void AddPlot(Bitmap plot, int columnStart, int rowStart, int columnEnd, int rowEnd)
        {

        }
    }
    
}
