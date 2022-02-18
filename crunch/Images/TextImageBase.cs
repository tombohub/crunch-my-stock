using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crunch.Images
{
    /// <summary>
    /// Base class for the text only images
    /// </summary>
    internal class TextImageBase
    {
        protected string Text;

        /// <summary>
        /// Font family to use in plots
        /// </summary>
        protected string FontFamily;

        /// <summary>
        /// Multiplot title font size
        /// </summary>
        protected float FontSize;

        /// <summary>
        /// Multiplier to affect multiplot dimensions and font sizes.
        /// </summary>
        protected float Scale;

        internal TextImageBase()
        {
            FontFamily = "Arial";
            Scale = 1;
        }

        /// <summary>
        /// Draw the general rectangle with desired text in center.
        /// Used for single metrics data and title.
        /// </summary>
        public Bitmap RenderImage(int width, int height)
        {

            // initialize objects
            Bitmap box = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(box);
            Rectangle rect = new Rectangle(0, 0, width, height);

            // make white background
            graphics.FillRectangle(Brushes.White, rect);

            // center text
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphics.DrawString(Text, new Font(FontFamily, FontSize), Brushes.Black, rect, format);

            return box;
        }
    }
}
