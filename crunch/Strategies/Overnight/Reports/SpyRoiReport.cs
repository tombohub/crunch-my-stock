using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Crunch.Database;
using Dapper;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.Reports
{
    internal class SpyRoiReport: IReport
    {
        /// <summary>
        /// SPY roi metric in percentage
        /// </summary>
        private decimal _spyRoi;

        public SpyRoiReport(decimal spyRoi)
        {
            _spyRoi = spyRoi;
        }

        /// <inheritdoc/>
        public Bitmap Plot(int width, int height)
        {
            string text = $"SPY ROI\n{_spyRoi}%";
            var plotter = new Plotter();

            var spyRoiPlot = plotter.RenderTextRectangle(text, width, height);
            return spyRoiPlot;
        }
    }
}
