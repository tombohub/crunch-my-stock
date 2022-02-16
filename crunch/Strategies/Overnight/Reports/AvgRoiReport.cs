using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Crunch.Database;
using Crunch.Images;
using Crunch.Domain.Multiplots;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class AvgRoiReport: IAreaContent
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
        public Bitmap RenderImage(int width, int height)
        {
            string text = $"Average ROI\n{_avgRoi}%";
            var plotter = new Plotter();
            var avgRoiPlot = plotter.RenderTextRectangle(text, width, height);

            return avgRoiPlot;
        }
    }
}
