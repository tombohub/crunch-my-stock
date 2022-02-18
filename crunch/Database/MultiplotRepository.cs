using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Multiplots;
using Crunch.Domain;
using Npgsql;
using Dapper;

namespace Crunch.Database
{
    internal class MultiplotRepository
    {
        NpgsqlConnection _connection;
        private StrategyName _strategy;
        private DateOnly _date;

        public MultiplotRepository(StrategyName strategy, DateOnly date)
        {
            _connection = DbConnections.CreatePsqlConnection();
            _strategy = strategy;
            _date = date;
        }

        public Multiplot GetMultiplot()
        {
            List<Area> areas = GetAreas();
            return new Multiplot(areas);
        }

        private List<Area> GetAreas()
        {
            string sql = @"SELECT area_name, x, y, width, height FROM multiplot_coordinates 
                        WHERE strategy = @Strategy
                        AND is_included = true";
            var parameters = new { Strategy = _strategy.ToString() };
            List<AreaDTO> areasDto = _connection.Query<AreaDTO>(sql, parameters).ToList();

            List<Area> areas = new List<Area>();
            foreach (var areaDto in areasDto)
            {
                IAreaContent areaContent = GetAreaContent(areaDto.AreaName);
                Area area = new Area(areaDto.X, areaDto.Y, areaDto.Width, areaDto.Height, areaContent);
                areas.Add(area);

            }
            return areas;
        }

        private IAreaContent GetAreaContent(string areaName)
        {
            var reportRepo = new Strategies.Overnight.AreaContentsRepository(_date);
            Type repoType = typeof(Strategies.Overnight.AreaContentsRepository);
            MethodInfo[] methods = repoType.GetMethods();
            string methodName = methods.Where(m => m.GetCustomAttribute<AreaAttribute>()?.Name == areaName)
                    .Select(m => m.Name)
                    .FirstOrDefault();

            IAreaContent areaContent = null;
            if (methodName != null)
            {
                areaContent = (IAreaContent)repoType.GetMethod(methodName).Invoke(reportRepo, null);
            }
            return areaContent;
        }

    }
}
