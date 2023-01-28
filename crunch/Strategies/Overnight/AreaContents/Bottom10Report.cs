using System.Collections.Generic;
using System.Drawing;
using Crunch.Core;
using Crunch.Core.Multiplots;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.AreaContents
{
    internal class Bottom10Report : IAreaContent
    {
        private List<SecurityPerformance> _reportData;

        public Bottom10Report(List<SecurityPerformance> reportData)
        {
            _reportData = reportData;
        }

        /// <summary>
        /// Plot Bottom 10 securities based on ROI
        /// </summary>
        /// <inheritdoc/>
        public Bitmap RenderImage(int width, int height)
        {
            var plotter = new Plotter();
            Bitmap plot = plotter.PlotBottom10Bars(_reportData, width, height);
            return plot;
        }
    }
}