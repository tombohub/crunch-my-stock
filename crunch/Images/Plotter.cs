using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Crunch.Domain;
using ScottPlot.Plottable;
using ScottPlot;

namespace Crunch.Images
{

    internal class Plotter
    {
        // TODO: scale should be in constructor
        /// <summary>
        /// Multiplier for scaling the sizes of plots and fonts.
        /// </summary>
        private float _scale;

        /// <summary>
        /// Font family to use in plots
        /// </summary>
        private string _fontFamily;

        /// <summary>
        /// Font size for single metric box
        /// </summary>
        private float _singleMetricFontSize;

        /// <summary>
        /// Individual plot title font size
        /// </summary>
        private float _plotTitleFontSize;

        /// <summary>
        /// Tick labels are primarly numbers on X and Y axis
        /// </summary>
        private float _tickLabelsFontSize;

        private float _legendFontSize;

        /// <summary>
        /// Color to visualize winning securities and positive numbers in general
        /// </summary>
        private Color _winnerColor;

        /// <summary>
        /// Color to visualize losing securities and negative numbers in general
        /// </summary>
        private Color _loserColor;

        public Plotter()
        {
            // TODO: make scale dynamic
            _fontFamily = "Arial";
            _singleMetricFontSize = 20;
            _plotTitleFontSize = 18;
            _tickLabelsFontSize = 16; 
            _legendFontSize = 16;
            _winnerColor = Color.ForestGreen;
            _loserColor = Color.Crimson;
        }

        /// <summary>
        /// Draw the general rectangle with desired text in center.
        /// Used for single metrics data and title.
        /// </summary>
        public Bitmap RenderTextRectangle(string text, int width, int height)
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

            graphics.DrawString(text, new Font(_fontFamily, _singleMetricFontSize), Brushes.Black, rect, format);

            return box;
        }

        /// <summary>
        /// Plot pie with winners vs losers securities count.
        /// </summary>
        /// <param name="reportData">value object containing report data</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot image</returns>
        public Bitmap PlotWinnersLosersPie(WinnersLosersCount reportData, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Winners Losers Total");
            SetFontSizes(plt);

            double[] values = { reportData.WinnersCount, reportData.LosersCount };
            string[] labels = { "Winners", "Losers" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.SliceFillColors = new Color[] { _winnerColor, _loserColor };

            return plt.Render();
        }

        /// <summary>
        /// Plot bars with winners vs losers securities count.
        /// </summary>
        /// <param name="reportData">value object containing report data</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot image</returns>
        public Bitmap PlotWinnersLosersBar(WinnersLosersCount reportData, int width, int height)
        {
            var plt = new ScottPlot.Plot(width,height);
            plt.Title("Winners Losers Total");
            SetFontSizes(plt);

            double[] values = {reportData.WinnersCount, reportData.LosersCount};
            string[] labels = { "Winners", "Losers" };
            
            var bar = plt.AddBar(values);
            

            return plt.Render();
        }

        public Bitmap PlotWinnersLosersGroupBars(WinnersLosersCount reportData, int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Winners Losers Total");
            SetFontSizes(plt);

            // only one group name
            string[] groupNames = { "Total" };
            string[] seriesNames = { "Winners", "Losers" };
            double[] winnersCount = { reportData.WinnersCount };
            double[] losersCount = { reportData.LosersCount };
            double[][] values = { winnersCount, losersCount };

            // null means no error values. Error values are required in plot method, but we dont wanna plot them.
            var barGroups = plt.AddBarGroups(groupNames, seriesNames, values, null);
            barGroups[0].FillColor = _winnerColor;
            barGroups[1].FillColor = _loserColor;
            barGroups[0].ShowValuesAboveBars = true;
            barGroups[1].ShowValuesAboveBars = true;

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
            SetFontSizes(plt);

            double[] values = orderedBottom10
                .Select(b => b.ChangePct)
                .ToArray();

            string[] labels = orderedBottom10
                .Select(b => b.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, _loserColor);
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

            return plt.Render();
        }

        /// <summary>
        /// Plot horizontal bars to the right showing top 10 securities
        /// </summary>
        /// <param name="reportData">Data containing list of securities with ROIs</param>
        /// <param name="width">Width of plot in pixels</param>
        /// <param name="height">Height of plot in pixels</param>
        /// <returns>Plot bitmap image</returns>
        public Bitmap PlotTop10Bars(List<SecurityPerformance> reportData, int width, int height)
        {
            ScottPlot.Plot plt = new(width, height);
            SetFontSizes(plt);

            List<SecurityPerformance> orderedTop10 = reportData.OrderBy(t => t.ChangePct).ToList();

            // bars overnight roi
            double[] values = orderedTop10
                .Select(t => t.ChangePct)
                .ToArray();

            string[] labels = orderedTop10
                .Select(t => t.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, color: _winnerColor);
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

        /// <summary>
        /// Plot Winners and Losers count grouped by manually selected price ranges.
        /// </summary>
        /// <param name="reportData">Data to plot</param>
        /// <param name="width">Plot width (pixels)</param>
        /// <param name="height">Plot height (pixels)</param>
        /// <returns>Rendered plot image</returns>
        public Bitmap PlotWinnersLosersCountByPrice(List<WinnersLosersCountByPrice> reportData, int width, int height)
        {
            var plt = new Plot(width, height);
            plt.Title("Winners vs Losers by price");
            SetFontSizes(plt);
            
            string[] groupNames = reportData.Select(s => s.PriceRange).ToArray();
            string[] seriesName = { "Winners", "Losers" };
            double[] winnersCounts = reportData.Select(s => (double)s.WinnersCount).ToArray();
            double[] losersCounts = reportData.Select(s => (double)s.LosersCount).ToArray();
            double[][] countValues = { winnersCounts, losersCounts };
            double[][] errors = { new double[winnersCounts.Length], new double[losersCounts.Length] };

            var barPlots = plt.AddBarGroups(groupNames, seriesName, countValues, errors);
            barPlots[0].FillColor = _winnerColor;
            barPlots[1].FillColor = _loserColor;

            plt.YAxis.Label("Count");
         
            var legend = plt.Legend();
            legend.Location = Alignment.UpperRight;
            legend.FontSize = _legendFontSize;

            return plt.Render();
        }

        private void SetFontSizes(Plot plt)
        {
            plt.XAxis2.LabelStyle(fontSize: _plotTitleFontSize);
            plt.XAxis.TickLabelStyle(fontSize: _tickLabelsFontSize);
            plt.YAxis.TickLabelStyle(fontSize:_tickLabelsFontSize);
        }
    }
}
