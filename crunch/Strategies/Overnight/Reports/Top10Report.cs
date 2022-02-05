using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using System.Drawing;
using Crunch.Plots;

namespace Crunch.Strategies.Overnight.Reports
{
    internal class Top10Report : IReport
    {
            private List<SecurityPerformance> _reportData;

        public Top10Report(List<SecurityPerformance> reportData)
        {
            _reportData = reportData;
        }

        /// <summary>
        /// Plot Top 10 securities based on ROI
        /// </summary>
        /// <inheritdoc/>
        public Bitmap Plot(int width, int height)
        {
            var plotter = new Plotter();
            Bitmap plot = plotter.PlotTop10Bars(_reportData, width, height);
            return plot;
        }
    }
    
}
