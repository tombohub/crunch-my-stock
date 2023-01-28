using System.Drawing;
using Crunch.Core;
using Crunch.Core.Multiplots;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.AreaContents
{
    /// <summary>
    /// Represents report for Winners and Losers count
    /// </summary>
    public class WinnersLosersReport : IAreaContent
    {
        private WinnersLosersCount _reportData;

        internal WinnersLosersReport(WinnersLosersCount reportData)
        {
            _reportData = reportData;
        }

        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <inheritdoc/>
        public Bitmap RenderImage(int width, int height)
        {
            var plotter = new Plotter();
            var plot = plotter.PlotWinnersLosersGroupBars(_reportData, width, height);
            return plot;
        }
    }
}