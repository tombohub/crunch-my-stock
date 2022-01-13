using Crunch.Database;
using System;
using System.Drawing;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Services provided by Overnight strategy
    /// </summary>
    class OvernightStrategyService : IStrategyService
    {

        public void Plot()
        {
            var multiplotSize = Helpers.GetMultiplotSize(Strategy.Overnight);
            Bitmap multiplot = new Bitmap(multiplotSize.Width, multiplotSize.Height);
            Console.WriteLine(multiplotSize);
        }
    }
}
