using Crunch.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using Crunch.Domain;
using Crunch.Strategies.Overnight.Multiplot;
using Crunch.Domain.Multiplots;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Services provided by Overnight strategy
    /// </summary>
    class OvernightStrategyService : IStrategyService
    {

        public void CreateStrategyMultiplot(DateOnly date)
        {
            var overnightDb   = new OvernightDatabase();
            List<AreaDTO> includedAreasDto = overnightDb.GetIncludedAreas();
            List<Area> areas = new List<Area>();
            foreach (AreaDTO areaDto in includedAreasDto)
            {
                Area area = AreaFactory.CreateArea(areaDto, date);
                areas.Add(area);
            }

            var multiplot = new Domain.Multiplots.Multiplot(areas);
            multiplot.SaveToFile("D:\\PROJEKTI\\moko.png");

            //var reportRepo = new ReportRepository(date);
            //foreach (var includedArea in includedAreas)
            //{
            //    IReport report = reportRepo.CreateReport(includedArea.AreaName);
            //    Bitmap reportPlot = report.Plot(includedArea.Width, includedArea.Height);
            //    multiplotGraphics.DrawImage(reportPlot, includedArea.X, includedArea.Y);
            //}
            //Multiplot multiplot = new Multiplot(includedAreas);
            //multiplot.Save("D:\\PROJEKTI\\noant.png");
        }
    }
}
