using System;
using System.Drawing;
using Crunch.Domain;
using Crunch.Database;
using Dapper;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// Represents report for Winners and Losers count
    /// </summary>
    public class WinnersLosersReport : IReport
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
        public Bitmap Plot(int width, int height)
        {
            var plotter = new Plotter();
            var plot = plotter.PlotWinnersLosersGroupBars(_reportData, width, height);
            return plot;
        }


    }
}
