using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;

//TODO: svaki plot ima dimenzije, dimenzije moraju biti unutar dimenzija generalnog plota
namespace Crunch.Strategies.Overnight
{
    class OvernightPlot
    {
        public OvernightPlot()
        {

        }

        public void PlotEverything(Reports reports)
        {
            Bitmap winnersLosersPlot = PlotWinnersLosers(reports.WinnersLosersRatio, 300, 300);
            Bitmap top10Plot = PlotTop10(reports.Top10, 300, 300);
            Bitmap bottom10Plot = PlotBottom10(reports.Bottom10, 300, 300);
            Bitmap spyBenchRoiBox = DrawSpyBenchmarkRoi(reports.SpyBenchmarkRoi, 300, 300);
            Bitmap spyOvernightRoiBox = DrawSpyOvernightRoi(reports.SpyOvernightRoi, 300, 300);
            Bitmap avgBenchRoiBox = DrawAverageBenchmarkRoi(reports.AverageBenchmarkRoi, 300, 300);
            Bitmap avgOvernightRoiBox = DrawAverageOvernightRoi(reports.AverageOvernightRoi, 300, 300);

            Bitmap plot = new Bitmap(800, 1600);
            var graphics = Graphics.FromImage(plot);
            graphics.DrawImage(top10Plot, 200, 0);
            graphics.DrawImage(bottom10Plot, 400, 0);

            plot.Save("D:\\PROJEKTI\\koko.png");
        }

        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="width">Plot width in pixels</param>
        /// <param name="height">Plot height in pixels</param>
        /// <param name="winLosData">Plot data</param>
        public Bitmap PlotWinnersLosers(WinnersLosersRatioReport winLosData, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);

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
        /// <param name="height"></param>
        public Bitmap PlotTop10(List<Top10Report> top10Data, int width, int height)
        {
            ScottPlot.Plot plt = new(width, height);

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
        public Bitmap PlotBottom10(List<Bottom10Report> bottom10Data, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);

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
        private Bitmap DrawRoiBox(string text, int width, int height)
        {
            // initialize objects
            Bitmap box = new Bitmap(width, height);
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
        public Bitmap DrawSpyBenchmarkRoi(double spyRoi, int width, int height)
        {
            string text = $"SPY\n{spyRoi:P2}";
            Bitmap spyBenchRoi = DrawRoiBox(text, width, height);

            return spyBenchRoi;
        }

        /// <summary>
        /// Draw Spy Overnight strategy ROI
        /// </summary>
        /// <param name="spyRoi"></param>
        public Bitmap DrawSpyOvernightRoi(double spyRoi, int width, int height)
        {
            string text = $"SPY Overnight\n{spyRoi:P2}";
            Bitmap spyOvernightRoi = DrawRoiBox(text, width, height);

            return spyOvernightRoi;
        }

        /// <summary>
        /// Draw Average ROI across all securities
        /// </summary>
        /// <param name="averageRoi"></param>
        public Bitmap DrawAverageOvernightRoi(double averageRoi, int width, int height)
        {
            string text = $"Average ROI\n{averageRoi:P2}";
            Bitmap avgOvernightRoi = DrawRoiBox(text, width, height);

            return avgOvernightRoi;
        }

        /// <summary>
        /// Draw average ROI of buy and hold strategy across all securities
        /// </summary>
        /// <param name="averageRoi"></param>
        public Bitmap DrawAverageBenchmarkRoi(double averageRoi, int width, int height)
        {
            string text = $"Buy Hold ROI\n{averageRoi:P2}";
            Bitmap avgBenchRoi = DrawRoiBox(text, width, height);

            return avgBenchRoi;
        }
    }

}
