using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Crunch.Database;
using Crunch.Plots;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class AvgRoiReport: IReport
    {
        /// <summary>
        /// Average roi metric in percentage
        /// </summary>
        private decimal _avgRoi;

        /// <summary>
        /// Initialize Average ROI Report
        /// </summary>
        /// <param name="avgRoi">Average ROI metric</param>
        public AvgRoiReport(decimal avgRoi)
        {
            _avgRoi = avgRoi;
        }

        /// <inheritdoc/>
        public Bitmap Plot(int width, int height)
        {
            string text = $"Average ROI\n{_avgRoi}%";
            var plotter = new Plotter();
            var avgRoiPlot = plotter.DrawMetricBox(text, width, height);

            return avgRoiPlot;
        }
    }
}
