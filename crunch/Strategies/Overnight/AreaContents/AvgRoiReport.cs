using System.Drawing;
using Crunch.Core.Multiplots;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.AreaContents
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class AvgRoiReport : IAreaContent
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