using Crunch.Domain;
using Crunch.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.DataSources.Fmp.HistoricalPricesEndpoint;
using Crunch.UseCases;
using Crunch.Domain.OhlcPrice;

namespace Crunch.DataSources
{
    internal class PriceDataSource
    {
        public PriceSet DownloadData(string symbol, DateOnly start, DateOnly end, PriceInterval interval)
        {
            HistoricalPricesRequester requester = new();
            PriceSet priceSet = requester.RequestData(symbol,interval, start, end);
            return priceSet;
        }
    }
}
