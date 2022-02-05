using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Crunch.Domain;
using ScottPlot.Plottable;
using ScottPlot;

namespace Crunch.Plots
{
    internal class Plotter
    {
        /// <summary>
        /// Draw the general rectangle with desired text in center.
        /// Used for single metrics data.
        /// </summary>
        public Bitmap DrawMetricBox(string text, int width, int height)
        {
            // initialize objects
            Bitmap box = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(box);
            Rectangle rect = new Rectangle(0, 0, width, height);

            // make white background
            graphics.FillRectangle(Brushes.White, rect);

            // center text
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphics.DrawString(text, new Font("Verdana", 24), Brushes.Black, rect, format);

            return box;
        }

        /// <summary>
        /// Plot pie with winners vs losers securuties count.
        /// </summary>
        /// <param name="winnersCount">Number of winning securities</param>
        /// <param name="losersCount">Number of losing securities</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot image</returns>
        public Bitmap PlotWinnersLosersPie(int winnersCount, int losersCount, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            double[] values = { winnersCount, losersCount };
            string[] labels = { "Winners", "Losers" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;
            pie.SliceFillColors = new Color[] { Color.DarkGreen, Color.DarkRed };
            
            return plt.Render();
        }

        /// <summary>
        /// Plot horizontal bars to the left showing bottom 10 securities
        /// </summary>
        /// <param name="reportData">Data containing list of securities with ROIs</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot bitmap image</returns>
        public Bitmap PlotBottom10Bars(List<SecurityPerformance> reportData, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            // order in list will affect the order in plot
            List<SecurityPerformance> orderedBottom10 = reportData.OrderBy(b => b.ChangePct).ToList();

            double[] values = orderedBottom10
                .Select(b => b.ChangePct)
                .ToArray();

            string[] labels = orderedBottom10
                .Select(b => b.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, Color.IndianRed);
            bar.FillColorNegative = bar.FillColor;
            bar.Label = "Overnight";
            bar.Orientation = Orientation.Horizontal;

            /// CODE FOR benchmark roi ticks, KEEP HeRE for now
            //lines benchmark ROI
            //double[] benchmarkRois = orderedBottom10
            //    .Select(b => (double)b.BenchmarkRoi)
            //    .ToArray();

            //double[] symbolPositions = Enumerable.Range(0, _reportData.Count)
            //    .Select(s => (double)s)
            //    .ToArray();

            //ScatterPlot benchmarkLines = plt.AddScatterPoints(
            //    benchmarkRois,
            //    symbolPositions,
            //    Color.Black,
            //    11,
            //    MarkerShape.verticalBar,
            //    "Buy & Hold");

            // adds % sign to tick number because the value is already
            // in percentage
            string AddPctToTick(double tick)
            {
                return $"{tick}%";
            }

            plt.Title("Bottom 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat(AddPctToTick);

            // XAxis2 is plot title
            plt.XAxis2.LabelStyle(fontSize: 50);

            return plt.Render();
        }

        /// <summary>
        /// Plot horizontal bars to the right showing top 10 securities
        /// </summary>
        /// <param name="reportData">Data containing list of securities with ROIs</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot bitmap image</returns>
        public Bitmap PlotTop10Bars(List<SecurityPerformance> top10Data, int width, int height)
        {
            ScottPlot.Plot plt = new(width, height);

            List<SecurityPerformance> orderedTop10 = top10Data.OrderBy(t => t.ChangePct).ToList();

            // bars overnight roi
            double[] values = orderedTop10
                .Select(t => t.ChangePct)
                .ToArray();

            string[] labels = orderedTop10
                .Select(t => t.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, color: Color.DarkGreen);
            bar.Orientation = Orientation.Horizontal;

            // adds % sign to tick number because the value is already
            // in percentage
            string AddPctToTick(double tick)
            {
                return $"{tick}%";
            }

            plt.Title("Top 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat(AddPctToTick);
            plt.Legend();

            return plt.Render();
        }

        public Bitmap PlotWinnersLosersCountByPrice(List<WinnersLosersCountByPrice> reportData, int width, int height)
        {
            var plt = new Plot(width, height);
            plt.Title("Winners vs Losers by price");
            
            string[] groupNames = reportData.Select(s => s.PriceUpperBound.ToString()).ToArray();
            string[] seriesName = { "Winners", "Losers" };
            double[] winnersCounts = reportData.Select(s => (double)s.WinnersCount).ToArray();
            double[] losersCounts = reportData.Select(s => (double)s.LosersCount).ToArray();
            double[][] countValues = { winnersCounts, losersCounts };
            double[][] errors = { new double[winnersCounts.Length], new double[losersCounts.Length] };

            plt.AddBarGroups(groupNames, seriesName, countValues, errors);

            return plt.Render();
        }
    }
}
