using Crunch.Database;
using Crunch.Domain;
using Crunch.Strategies.Overnight;
using Crunch.Strategies.Overnight.Plots;
using Crunch.Strategies.Overnight.Reports;
using System.Collections.Generic;

namespace Crunch.UseCases
{
    /// <summary>
    /// Use Cases responsible for creating plots
    /// </summary>
    class PlottingUseCases
    {
        /// <summary>
        /// Object for calculating reports data
        /// </summary>
        private ReportsCalculator _reportsCalculator;


        public PlottingUseCases(int weekNum)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);
            _reportsCalculator = new ReportsCalculator(overnightStats);
        }

        public void PlotWinnersLosersUseCase(SecurityType securityType)
        {
            WinnersLosersRatioReport winLosData = _reportsCalculator.CalculateWinnersLosersRatio(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.PlotWinnersLosers(winLosData, 300, 300);
        }

        public void PlotTop10UseCase(SecurityType securityType)
        {
            List<Strategies.Overnight.SingleSymbolStats> top10Data = _reportsCalculator.CalculateTop10(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.PlotTop10(top10Data, 300, 300);
        }

        public void PlotBottom10UseCase(SecurityType securityType)
        {
            List<Strategies.Overnight.SingleSymbolStats> bottom10ReportData = _reportsCalculator.CalculateBottom10(securityType);
            Bottom10Report bottom10Report = new Bottom10Report(bottom10ReportData);
            bottom10Report.Plot(300, 300);
        }

        public void DrawSpyBenchmarkRoiUseCase()
        {
            double spyRoi = _reportsCalculator.GetSpyBenchmarkRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawSpyBenchmarkRoi(spyRoi, 300, 300);
        }

        public void DrawSpyOvernightRoiUseCase()
        {
            double spyRoi = _reportsCalculator.GetSpyOvernightRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawSpyOvernightRoi(spyRoi, 300, 300);
        }

        public void DrawAverageOvernightRoiUseCase(SecurityType securityType)
        {
            double avgRoi = _reportsCalculator.CalculateAverageOvernightRoi(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawAverageOvernightRoi(avgRoi, 300, 300);
        }

        public void DrawAverageBenchmarkRoiUseCase()
        {
            double avgRoi = _reportsCalculator.CalculateAverageBenchmarkRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawAverageBenchmarkRoi(avgRoi, 300, 300);
        }

        public void PlotOvernightUseCase(SecurityType securityType)
        {
            ReportsCollection reports = _reportsCalculator.CreateReports(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.CreateMultiplot(reports);
        }

    }
}
