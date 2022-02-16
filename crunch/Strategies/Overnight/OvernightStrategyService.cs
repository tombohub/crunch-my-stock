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
            var repo = new MultiplotRepository(StrategyName.Overnight, date);
            Multiplot multiplot = repo.GetMultiplot();
            multiplot.SaveToFile("D:\\PROJEKTI\\moko.png");
        }
    }
}
