using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Crunch.Core.Multiplots
{
    /// <summary>
    /// Represents final report for the strategy
    /// </summary>
    internal class Multiplot
    {
        /// <summary>
        /// Areas included in multiplot layout
        /// </summary>
        private List<Area> _areas;

        /// <summary>
        /// Title of the multiplot
        /// </summary>
        private string _title;

        /// <summary>
        /// Description for the multiplot
        /// </summary>
        private string _description;

        /// <summary>
        /// Create Multiplot instance with given width and height in pixels for the multiplot image
        /// </summary>
        /// <param name="width">Multiplot image width in pixels</param>
        /// <param name="height">Multiplot image height in pixels</param>
        public Multiplot(List<Area> areas)
        {
            _areas = areas;
        }

        public Multiplot()
        { }

        public void AddArea(Area area)
        {
            _areas.Add(area);
        }

        /// <summary>
        /// Save multiplot image to the file
        /// </summary>
        /// <param name="filename">filename path</param>
        public void SaveToFile(string filename)
        {
            Bitmap multiplotImage = CreateImage();
            multiplotImage.Save(filename);
        }

        /// <summary>
        /// Create the Bitmap image with correct size
        /// </summary>
        /// <returns>Bitmap object with calculated size</returns>
        private Bitmap CreateImage()
        {
            Size size = CalculateImageSize();
            var multiplotImage = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(multiplotImage);
            foreach (var area in _areas)
            {
                area.DrawContent(graphics);
            }
            return multiplotImage;
        }

        /// <summary>
        /// Calculate the Multiplot image size depending on sizes of included Areas
        /// </summary>
        /// <returns cref="Size">Size object with width and height in pixels</returns>
        /// <exception cref="Exception">In case the Areas field is null</exception>
        private Size CalculateImageSize()
        {
            if (_areas != null)
            {
                int width = _areas.Select(area => area.X + area.Width).Max();
                int height = _areas.Select(area => area.Y + area.Height).Max();
                return new Size(width, height);
            }
            else
            {
                throw new Exception("Areas are null");
            }
        }
    }
}