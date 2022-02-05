using Crunch.Database;
using System;
using System.Drawing;
using Crunch.Plots;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Services provided by Overnight strategy
    /// </summary>
    class OvernightStrategyService : IStrategyService
    {

        public void CreateStrategyMultiplot(DateOnly date)
        {
            var multiplotSize = Helpers.GetMultiplotSize(Strategy.Overnight);
            Bitmap multiplot = new Bitmap(multiplotSize.Width, multiplotSize.Height);
            using Graphics multiplotGraphics = Graphics.FromImage(multiplot);
            var plotter = new Plotter();

            // foreach report in reports
            var overnightDb   = new OvernightDatabase();
            var includedPlots = overnightDb.GetIncludedPlotsCoordinates();
            var reportRepo = new ReportRepository(date);
            foreach (var includedPlot in includedPlots)
            {
                var report = reportRepo.CreateReport(includedPlot.Report);
                var reportPlot = report.Plot(includedPlot.Width, includedPlot.Height);
                multiplotGraphics.DrawImage(reportPlot, includedPlot.X, includedPlot.Y);
            }
            multiplot.Save("D:\\PROJEKTI\\noant.png");
        }
    }
}
