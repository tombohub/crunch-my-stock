using System.Drawing;
using Crunch.Domain;

namespace Crunch.Domain.Multiplots
{
    /// <summary>
    /// Individual area inside the multiplot.
    /// Area can be plot, title or other element of the finished multiplot image.
    /// Has coordinates and dimensions.
    /// </summary>
    internal record Area
    {
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

        /// <summary>
        /// Rectangle representing the location and dimensions
        /// of area inside multiplot
        /// </summary>
        internal Rectangle Rect { get; init; }

        /// <summary>
        /// Image which is rendered in the current Area
        /// </summary>
        internal Bitmap Image { get; set; }

        internal Area(int x, int y, int width, int height, Bitmap image)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Image = image;
        }

        internal Area(Rectangle rect, Bitmap image)
        {
            Rect = rect;
            Image = image;
        }

        internal void DrawImage()
        {
            Image = new Bitmap(Width, Height);
        }

    }
}
