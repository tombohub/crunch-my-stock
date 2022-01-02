using ScottPlot;
using ScottPlot.Plottable;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crunch.Strategies.Overnight.Reports;

//TODO: svaki plot ima dimenzije, dimenzije moraju biti unutar dimenzija generalnog plota
namespace Crunch.Strategies.Overnight.Plots
{
    /// <summary>
    /// Class fror creating overnight report
    /// </summary>
    class OvernightPlotter
    {
        /// <summary>
        /// Multiplot image width in pixels
        /// </summary>
        private int _multiplotWidth = 1200;


        public OvernightPlotter()
        {

        }

        /// <summary>
        /// Composes all individual plots into one
        /// </summary>
        /// <param name="reports"></param>
        public void CreateMultiplot(ReportsCollection reports)
        {
            var overnightLayout = new GridLayout(1200, 5, 4);
            int columnsNumber = 8;

            int rowsNumber = 12;
            int gridSquareSize = _multiplotWidth / columnsNumber;
            int multiplotHeight = rowsNumber * gridSquareSize;
            Bitmap multiPlot = new Bitmap(_multiplotWidth, multiplotHeight);
            var graphics = Graphics.FromImage(multiPlot);

            (int Width, int Height) avgOvernightRoiBoxSize = (2, 1);


            Bitmap avgOvernightRoiBox = DrawAverageOvernightRoi(reports.AverageOvernightRoi, avgOvernightRoiBoxSize.Width * gridSquareSize, avgOvernightRoiBoxSize.Height * gridSquareSize);
            Bitmap avgBenchRoiBox = DrawAverageBenchmarkRoi(reports.AverageBenchmarkRoi, 2 * gridSquareSize, 1 * gridSquareSize);
            Bitmap spyOvernightRoiBox = DrawSpyOvernightRoi(reports.SpyOvernightRoi, 2 * gridSquareSize, 1 * gridSquareSize);
            Bitmap spyBenchRoiBox = DrawSpyBenchmarkRoi(reports.SpyBenchmarkRoi, 2 * gridSquareSize, 1 * gridSquareSize);
            var winnersLosersReport = new WinnersLosersReport();
            Bitmap winnersLosersPlot = winnersLosersReport.Plot(4 * gridSquareSize, 4 * gridSquareSize);
            Bitmap top10Plot = PlotTop10(reports.Top10, 4 * gridSquareSize, 4 * gridSquareSize);
            //Bitmap bottom10Plot = PlotBottom10(reports.Bottom10, 4 * gridSquareSize, 4 * gridSquareSize);
            // todo: activate bottom10plot

            graphics.DrawImage(avgOvernightRoiBox, overnightLayout.GetArea(0, 0, 0, 0));
            graphics.DrawImage(avgBenchRoiBox, overnightLayout.GetArea(0, 1, 0, 1));
            graphics.DrawImage(spyOvernightRoiBox, overnightLayout.GetArea(0, 2, 0, 2));
            graphics.DrawImage(spyBenchRoiBox, overnightLayout.GetArea(0, 3, 0, 3));
            graphics.DrawImage(winnersLosersPlot, overnightLayout.GetArea(1, 0, 2, 1));
            graphics.DrawImage(top10Plot, overnightLayout.GetArea(3, 0, 4, 1));
            //graphics.DrawImage(bottom10Plot, overnightLayout.GetArea(3,2,4,3));


            multiPlot.Save("D:\\PROJEKTI\\koko.png");
        }

        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="width">Plot width in pixels</param>
        /// <param name="height">Plot height in pixels</param>
        /// <param name="winLosData">Plot data</param>
        public Bitmap PlotWinnersLosers(Overnight.Reports.WinnersLosersReport winLosData, int width, int height)
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
        public Bitmap PlotTop10(List<SingleSymbolStats> top10Data, int width, int height)
        {
            ScottPlot.Plot plt = new(width, height);

            List<SingleSymbolStats> orderedTop10 = top10Data.OrderBy(t => t.OvernightRoi).ToList();

            // bars overnight roi
            double[] values = orderedTop10
                .Select(t => t.OvernightRoi)
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
        /// Draw Average ROI across all securities with size in pixels
        /// </summary>
        /// <param name="averageRoi"></param>
        /// <param name="width">width in pixels</param>
        /// <param name="height">height in pixels</param>
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
