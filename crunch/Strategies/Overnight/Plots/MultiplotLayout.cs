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
        /// Collection of individual grid cells
        /// </summary>
        private Rectangle[,] _cells;

        /// <summary>
        /// Initialize multiplot layout grid object.
        /// </summary>
        /// <param name="imageWidth">Width of multiplot in pixels</param>
        /// <param name="rowsCount">Number of rows in grid</param>
        /// <param name="columnsCount">Number of columns in grid</param>
        public MultiplotLayout(int imageWidth, int rowsCount, int columnsCount)
        {
            _cells = CreateCells(imageWidth, rowsCount, columnsCount);
        }

        /// <summary>
        /// Get area which contains one or more grid cells
        /// </summary>
        /// <param name="startRow">0 based row number for starting grid cell (top left)</param>
        /// <param name="startColumn">0 based column number for starting grid cell (top left)</param>
        /// <param name="endRow">0 based row number for ending grid cell (bottom right)</param>
        /// <param name="endColumn">0 based column number fro ending grid cell (bottom right)</param>
        /// <returns>Rectangle object</returns>
        public Rectangle GetArea(int startRow, int startColumn, int endRow, int endColumn)
        {
            // note: pixels start from top left corner
            Rectangle startCell = _cells[startRow, startColumn];
            Rectangle endCell = _cells[endRow, endColumn];
            int startX = startCell.X;
            int startY = startCell.Y;
            int endX = endCell.Right;
            int endY = endCell.Bottom;
            
            int areaWidth = endX - startX;
            int areaHeight = endY - startY;

            return new Rectangle(startX, startY, areaWidth, areaHeight);
        }

        /// <summary>
        /// Create an 2d array of individual grid cells
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="rowsCount"></param>
        /// <param name="columnsCount"></param>
        /// <returns></returns>
        private Rectangle[,] CreateCells(int imageWidth, int rowsCount, int columnsCount)
        {
            int cellSideLength = imageWidth / columnsCount;
            Rectangle[,] cells = new Rectangle[rowsCount, columnsCount];
            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    int startX = columnNumber * cellSideLength;
                    int startY = rowNumber * cellSideLength;
                    cells[rowNumber, columnNumber] = new Rectangle(startX, startY, cellSideLength, cellSideLength);
                }
            }

            return cells;
        }
    }
    
}
