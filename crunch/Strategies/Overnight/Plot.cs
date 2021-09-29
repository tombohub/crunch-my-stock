using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

namespace Crunch.Strategies.Overnight
{
    class Plot
    {
        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="weekNum"></param>
        public static Bitmap PlotWinnersLosers(WinnersLosersRatioReport winLosData)
        {
            var plt = new ScottPlot.Plot();

            double[] values = { winLosData.WinnersCount, winLosData.LosersCount };
            string[] labels = { "Winners", "Losers" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;
            pie.SliceFillColors = new Color[] { Color.DarkGreen, Color.DarkRed };
            pie.Explode = true;

            return plt.Render();
        }

        /// <summary>
        /// Plot Weekly Overnight top 10 ROI
        /// </summary>
        public static Bitmap PlotTop10(List<Top10Report> top10Data)
        {
            ScottPlot.Plot plt = new(600, 400);

            List<Top10Report> orderedTop10 = top10Data.OrderByDescending(t => t.StrategyRoi).ToList();

            // bars overnight roi
            double[] values = orderedTop10
                .Select(t => t.StrategyRoi)
                .ToArray();

            string[] labels = orderedTop10
                .Select(t => t.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, color: Color.DarkGreen);
            bar.Label = "Overnight";
            bar.Orientation = Orientation.Horizontal;

            // lines benchmark roi
            double[] x = orderedTop10
                .Select(t => (double)t.BenchmarkRoi)
                .ToArray();

            double[] y = Enumerable.Range(0, top10Data.Count)
                .Select(y => (double)y)
                .ToArray();

            var benchmarkLines = plt.AddScatterPoints(x, y, Color.Black, 11, MarkerShape.verticalBar, "Buy & Hold");

            plt.Title("Top 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat("P1", false);
            plt.Legend();

            return plt.Render();
        }

        /// <summary>
        /// Plot Bottom 10 securities based on ROI
        /// </summary>
        /// <param name="bottom10Data"></param>
        public static Bitmap PlotBottom10(List<Bottom10Report> bottom10Data)
        {
            var plt = new ScottPlot.Plot(600, 400);

            List<Bottom10Report> orderedBottom10 = bottom10Data.OrderBy(b => b.StrategyRoi).ToList();

            double[] values = orderedBottom10
                .Select(b => b.StrategyRoi)
                .ToArray();

            string[] labels = orderedBottom10
                .Select(b => b.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, Color.IndianRed);
            bar.FillColorNegative = bar.FillColor;
            bar.Label = "Overnight";
            bar.Orientation = Orientation.Horizontal;

            //lines benchmark ROI
            double[] benchmarkRois = orderedBottom10
                .Select(b => (double)b.BenchmarkRoi)
                .ToArray();

            double[] symbolPositions = Enumerable.Range(0, bottom10Data.Count)
                .Select(s => (double)s)
                .ToArray();

            ScatterPlot benchmarkLines = plt.AddScatterPoints(
                benchmarkRois,
                symbolPositions,
                Color.Black,
                11,
                MarkerShape.verticalBar,
                "Buy & Hold");

            plt.Title("Bottom 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat("P1", false);
            plt.Legend(location: Alignment.UpperLeft);

            return plt.Render();
        }

        /// <summary>
        /// Draw the rectangle with text in center
        /// </summary>
        /// <param name="text"></param>
        /// <param name="filename"></param>
        private static Bitmap DrawRoiBox(string text, string filename)
        {
            // initialize objects
            Bitmap box = new Bitmap(200, 200);
            Graphics graphics = Graphics.FromImage(box);

            // make white background
            graphics.FillRectangle(Brushes.White, 0, 0, box.Width, box.Height);

            // center text
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var rect = new Rectangle(0, 0, 200, 200);

            graphics.DrawString(text, new Font("Verdana", 18), Brushes.Black, rect, format);

            return box;
        }

        /// <summary>
        /// Draws the Spy benchmark ROI
        /// </summary>
        public static Bitmap DrawSpyBenchmarkRoi(double spyRoi)
        {
            string text = $"SPY\n{spyRoi:P2}";
            Bitmap spyBenchRoi = DrawRoiBox(text, "spybenchroi");

            return spyBenchRoi;
        }

        /// <summary>
        /// Draw Spy Overnight strategy ROI
        /// </summary>
        /// <param name="spyRoi"></param>
        public static Bitmap DrawSpyOvernightRoi(double spyRoi)
        {
            string text = $"SPY Overnight\n{spyRoi:P2}";
            Bitmap spyOvernightRoi = DrawRoiBox(text, "spyovernightroi");

            return spyOvernightRoi;
        }

        /// <summary>
        /// Draw Average ROI across all securities
        /// </summary>
        /// <param name="averageRoi"></param>
        public static Bitmap DrawAverageOvernightRoi(double averageRoi)
        {
            string text = $"Average ROI\n{averageRoi:P2}";
            Bitmap avgOvernightRoi = DrawRoiBox(text, "avgroi");

            return avgOvernightRoi;
        }

        /// <summary>
        /// Draw average ROI of buy and hold strategy across all securities
        /// </summary>
        /// <param name="averageRoi"></param>
        public static Bitmap DrawAverageBenchmarkRoi(double averageRoi)
        {
            string text = $"Buy Hold ROI\n{averageRoi:P2}";
            Bitmap avgBenchRoi = DrawRoiBox(text, "buyholdroi");

            return avgBenchRoi;
        }
    }
}
