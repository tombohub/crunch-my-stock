using System;
using System.Drawing;
using Crunch.Domain;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// Represents report for Winners and Losers count
    /// </summary>
    public class WinnersLosersReport : IReport
    {
        public DateOnly Date { get; set; }
        public SecurityType SecurityType { get; init; }
        public int WinnersCount { get; init; }
        public int LosersCount { get; init; }

        public WinnersLosersReport(int winnersCount, int losersCount)
        {
            WinnersCount = winnersCount;
            LosersCount = losersCount;
        }
        
        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="width">Plot width in pixels</param>
        /// <param name="height">Plot height in pixels</param>
        public Bitmap Plot(int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);

            double[] values = { WinnersCount, LosersCount };
            string[] labels = { "Winners", "Losers" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;
            pie.SliceFillColors = new Color[] { Color.DarkGreen, Color.DarkRed };
            pie.Explode = true;

            return plt.Render();
        }
    }
}
