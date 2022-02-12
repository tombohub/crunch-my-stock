using ScottPlot;
using ScottPlot.Plottable;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crunch.Domain;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.Reports
{
    internal class Bottom10Report : IReport
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
        public Bitmap Plot(int width, int height)
        {
            var plotter = new Plotter();
            Bitmap plot = plotter.PlotBottom10Bars(_reportData, width, height);
            return plot;
        }
    }
}
