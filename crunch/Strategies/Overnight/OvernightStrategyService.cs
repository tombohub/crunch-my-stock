using System;
using Crunch.Core;
using Crunch.Core.Multiplots;
using Crunch.Database;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Services provided by Overnight strategy
    /// </summary>
    internal class OvernightStrategyService : IStrategyService
    {
        public void CreateStrategyMultiplot(DateOnly date)
        {
            var repo = new MultiplotRepository(StrategyName.Overnight, date);
            Multiplot multiplot = repo.GetMultiplot();
            multiplot.SaveToFile("D:\\PROJEKTI\\moko.png");
        }
    }
}