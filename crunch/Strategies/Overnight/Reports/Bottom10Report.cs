using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot;
using ScottPlot.Plottable;
using Crunch.Domain.Strategies;

namespace Crunch.Strategies.Overnight.Reports
{
    public record Bottom10Report : IReport
    {
        public string Symbol { get; init; }
        public double StrategyRoi { get; init; }
        public double BenchmarkRoi { get; init; }

        private List<SingleSymbolStats> _reportData;

        public Bottom10Report(List<SingleSymbolStats> reportData)
        {
            _reportData = reportData;
        }

        /// <summary>
        /// Plot Bottom 10 securities based on ROI
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Bitmap Plot(int width, int height)
        {
            var plt = new ScottPlot.Plot(width, height);

            List<SingleSymbolStats> orderedBottom10 = _reportData.OrderBy(b => b.OvernightRoi).ToList();

            double[] values = orderedBottom10
                .Select(b => b.OvernightRoi)
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

            double[] symbolPositions = Enumerable.Range(0, _reportData.Count)
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
    }
}
