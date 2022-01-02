using Crunch.Domain.Strategies;
using System;
using System.Drawing;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Overnight strategy facade.
    /// </summary>
    class OvernightStrategyService : IStrategyService
    {

        public void Plot()
        {
            //TODO: implement dynamic height dependent on number of subplots
            Bitmap multiplot = new Bitmap(600, 1200);
            Console.WriteLine("Plotting Overnight");
        }
    }
}
