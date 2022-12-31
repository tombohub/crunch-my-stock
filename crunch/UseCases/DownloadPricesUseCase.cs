using System;
using System.Net;
using System.Threading;
using Crunch.Database;
using Crunch.DataSources;
using Crunch.Domain;

namespace Crunch.UseCases
{
    /// <summary>
    /// Download prices for stocks and etfs from data source and save them to database
    /// </summary>
    [Obsolete("Importing now from CrunchImport project")]
    internal class DownloadPricesUseCase : IUseCase
    {
        /// <summary>
        /// Data source prices provider object
        /// </summary>
        private PriceDataSource _source = new PriceDataSource();

        /// <summary>
        /// Period for which too download prices
        /// </summary>
        private readonly Period _dateRange;

        /// <summary>
        /// Price data interval
        /// </summary>
        private readonly PriceInterval _interval;

        /// <summary>
        /// Pause between data source API request in miliseconds.
        /// To avoid 'too many requests' error
        /// </summary>
        private readonly int _requestPause = 500;

        /// <summary>
        /// Initialize Import prices use case object.
        /// </summary>
        ///<param name="dateRange">Period for the price set</param>
        /// <param name="interval">Prices data interval between each price</param>
        public DownloadPricesUseCase(Period dateRange, PriceInterval interval)
        {
            _dateRange = dateRange;
            _interval = interval;
        }

        /// <summary>
        /// Execute Import prices use case
        /// </summary>
        public void Execute()
        {
            // get list of symbols
            var symbols = Helpers.GetSecuritySymbols();

            foreach (var symbol in symbols)
            {
                var thread = new Thread(() => ImportPrices(symbol));
                Thread.Sleep(_requestPause);
                thread.Start();
            }
        }

        /// <summary>
        /// Separate method encompassing the download and saving to database,
        /// so it can be used for threading process
        /// </summary>
        /// <param name="symbol"></param>
        private void ImportPrices(Symbol symbol)
        {
            DailyPriceSet priceSet = null;

            try
            {
                Console.WriteLine($"Requesting {symbol}");
                priceSet = _source.DownloadData(symbol, _dateRange, _interval);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (priceSet != null)
            {
                Console.WriteLine($"Saving {symbol}");

                //HACK: creating new instance of repo to make it thread safe
                DailyPriceSetRepository repo = new DailyPriceSetRepository();
                repo.SaveWithDapper(priceSet, _interval);
            }
            else Console.WriteLine($"{symbol} data is NULL");

            Console.WriteLine($"{symbol} imported.");
        }
    }
}