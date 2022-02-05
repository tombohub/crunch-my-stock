using System;
using System.Drawing;
using Crunch.Domain;
using Crunch.Plots;
using Crunch.Database;
using Dapper;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// Represents report for Winners and Losers count
    /// </summary>
    public class WinnersLosersReport : IReport
    {
        /// <summary>
        /// Number of securities with positive gain
        /// </summary>
        private int _winnersCount;

        /// <summary>
        /// Number of securities with negative gain
        /// </summary>
        private int _losersCount;

        public WinnersLosersReport(int winnersCount, int losersCount)
        {
            _winnersCount = winnersCount;
            _losersCount = losersCount;
        }

        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <inheritdoc/>
        public Bitmap Plot(int width, int height)
        {
            var plotter = new Plotter();
            var plot = plotter.PlotWinnersLosersPie(_winnersCount, _losersCount, width, height);
            return plot;
        }


    }
}
