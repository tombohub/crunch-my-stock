using System.Drawing;

namespace Crunch.Strategies
{
    /// <summary>
    /// Represents report object.
    /// </summary>
    interface IReport
    {

        /// <summary>
        /// Create a plot for out of the report data
        /// </summary>
        /// <param name="width">width of the plot in pixels</param>
        /// <param name="height">height of the plot in pixels</param>
        /// <returns>Plot image</returns>
        public Bitmap Plot(int width, int height);
    }
}
