using System.Collections.Generic;
using System.Drawing;
using Crunch.Core;
using Crunch.Core.Multiplots;
using Crunch.Images;

namespace Crunch.Strategies.Overnight.AreaContents
{
    internal class WinnersLosersByPriceReport : IAreaContent
    {
        private List<WinnersLosersCountByPrice> _reportData;

        public WinnersLosersByPriceReport(List<WinnersLosersCountByPrice> reportData)
        {
            _reportData = reportData;
        }

        ///<summary>
        ///Plot comparison of number of winners and losers securities by security price
        /// </summary>
        /// <inheritdoc/>
        public Bitmap RenderImage(int width, int height)
        {
            var plotter = new Plotter();
            Bitmap plot = plotter.PlotWinnersLosersCountByPrice(_reportData, width, height);
            return plot;
        }
    }
}