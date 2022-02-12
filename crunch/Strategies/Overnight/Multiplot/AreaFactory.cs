using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Strategies.Overnight.Reports;
using System.Drawing;
using Crunch.Domain.Multiplots;
using Crunch.Images;


namespace Crunch.Strategies.Overnight.Multiplot
{
    internal class AreaFactory
    {
        internal static Area CreateArea(AreaDTO areaDto, DateOnly date)
        {
            Rectangle rect = new Rectangle(areaDto.X, areaDto.Y, areaDto.Width, areaDto.Height);
            Size size = new Size(areaDto.Width, areaDto.Height);

            return areaDto.AreaName switch
            {
                // TODO: make proper dynamic title with date
                AreaName.Title => CreateTitleArea(areaDto, $"Overnight {date}"),
                AreaName.WinnersLosersByPrice => CreateReportArea(areaDto, date, ReportName.WinnersLosersByPrice),
                AreaName.AvgRoi => CreateReportArea(areaDto, date, ReportName.AvgRoi),
                AreaName.SpyRoi => CreateReportArea(areaDto, date, ReportName.SpyRoi),
                AreaName.WinnersLosers => CreateReportArea(areaDto, date,ReportName.WinnersLosers),
                AreaName.Top10 => CreateReportArea(areaDto, date, ReportName.Top10),
                AreaName.Bottom10 => CreateReportArea(areaDto, date, ReportName.Bottom10),
                _ => throw new NotImplementedException()
            };
        }

        private static Area CreateTitleArea(AreaDTO areaDto, string title)
        {
            var multiplotTitle = new MultiplotTitle(title, areaDto.Scale);
            Bitmap titleImage = multiplotTitle.Render(areaDto.Width, areaDto.Height);
            return new Area(areaDto.X, areaDto.Y, areaDto.Width, areaDto.Height, titleImage);
        }

        private static Area CreateReportArea(AreaDTO areaDto, DateOnly date, ReportName reportName)
        {
            var reportRepo = new ReportRepository(date);
            IReport report = reportRepo.CreateReport(reportName);
            Bitmap image = report.Plot(areaDto.Width, areaDto.Height);
            return new Area(areaDto.X, areaDto.Y, areaDto.Width, areaDto.Height, image);
        }
    }
}
