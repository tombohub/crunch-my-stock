﻿using System.Drawing;

namespace Crunch.Core.Multiplots
{
    /// <summary>
    /// Individual area inside the multiplot.
    /// Area can be plot, title or other element of the finished multiplot image.
    /// Has coordinates and dimensions.
    /// </summary>
    internal record Area
    {
        /// <summary>
        /// X coordinate of area
        /// </summary>
        public int X { get; init; }

        /// <summary>
        /// Y coordinate of area
        /// </summary>
        public int Y { get; init; }

        /// <summary>
        /// Width of area
        /// </summary>
        public int Width { get; init; }

        /// <summary>
        /// Height of area
        /// </summary>
        public int Height { get; init; }

        /// <summary>
        /// Object containing area content. It renders image to attach to area.
        /// </summary>
        private IAreaContent _areaContent;

        public Area(int x, int y, int width, int height, IAreaContent areaContent)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _areaContent = areaContent;
        }

        /// <summary>
        /// Draws area content image onto the multiplot.
        /// </summary>
        /// <param name="graphics">Graphics instance created from multiplot sized image</param>
        public void DrawContent(Graphics graphics)
        {
            graphics.DrawImage(_areaContent.RenderImage(Width, Height), X, Y);
        }
    }
}