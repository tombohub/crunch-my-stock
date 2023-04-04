using System.Drawing;
using Crunch.Core;

namespace Crunch
{
    internal class PlottingMethods
    {
        public Bitmap WinnersLosersPlot(WinnersLosersCount winnersLosers)
        {
            var plt = new ScottPlot.Plot(600, 400);
            double[] positions = { 0, 1 };
            string[] labels = { "Losers", "Winners" };
            var barLosers = plt.AddBar(new double[] { winnersLosers.LosersCount }, new double[] { 0 }, Color.DarkRed);
            var barWinners = plt.AddBar(new double[] { winnersLosers.WinnersCount }, new double[] { 1 }, Color.DarkGreen);
            plt.XTicks(positions, labels);
            barLosers.ShowValuesAboveBars = true;
            barWinners.ShowValuesAboveBars = true;

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
            Bitmap plotImage = plt.Render();

            return plotImage;
        }
    }
}