using Crunch.Database;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using Crunch.Domain;
using Crunch.Strategies.Overnight;
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
            var reportRepo = new ReportRepository(date);
            MethodInfo[] methods = reportRepo.GetType().GetMethods();
            List<Area> areas = new List<Area>();
            foreach (AreaDTO areaDto in includedAreasDto)
            {
                string methodName = methods.Where(m => m.GetCustomAttribute<AreaNameAttribute>()?.Name == areaDto.AreaName)
                    .Select(m => m.Name)
                    .FirstOrDefault();

                if (methodName != null)
                {
                    IAreaContent areaContent = (IAreaContent)reportRepo.GetType().GetMethod(methodName).Invoke(reportRepo, null);
                    Area area = new Area(areaDto.X, areaDto.Y, areaDto.Width, areaDto.Height, areaContent);
                    areas.Add(area);
                }

                
            }

            var multiplot = new Domain.Multiplots.Multiplot(areas);
            multiplot.SaveToFile("D:\\PROJEKTI\\moko.png");
        }
    }
}
