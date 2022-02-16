using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using System.Drawing;
using Crunch.Images;
using Crunch.Domain.Multiplots;

namespace Crunch.Strategies.Overnight.Reports
{
    internal class Top10Report : IAreaContent
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
        public Bitmap RenderImage(int width, int height)
        {
            var plotter = new Plotter();
            Bitmap plot = plotter.PlotTop10Bars(_reportData, width, height);
            return plot;
        }
    }
    
}
