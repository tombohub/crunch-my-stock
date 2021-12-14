using Crunch.Database;
using Crunch.DataSources;
using Crunch.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Crunch.UseCases
{
    /// <summary>
    /// Download prices for stocks and etfs from data source and save them to database
    /// </summary>
    internal class DownloadPricesUseCase : IUseCase
    {
        /// <summary>
        /// Data source prices provider object
        /// </summary>
        PriceDataSource _source = new PriceDataSource();

        /// <summary>
        /// Prices repository object
        /// </summary>
        //PriceSetRepository _repo = new PriceSetRepository();

        /// <summary>
        /// Prices data starting date
        /// </summary>
        private readonly DateOnly _startDate;

        /// <summary>
        /// Prices data ending date, inclusive
        /// </summary>
        private readonly DateOnly _endDate;

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
        /// <param name="start">Prices data starting date</param>
        /// <param name="end">Prices data ending date, inclusive</param>
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
            List<string> symbols = Helpers.GetSecuritySymbols();

            // truncate the database because EF core does not implement UPSERT
            // and overnight strategy has not implemented start date and end date calculation
            Helpers.TruncatePricesTable(_interval);

            foreach (string symbol in symbols)
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
        private void ImportPrices(string symbol)
        {
            DailyPriceSet priceSet = null;

            try
            {
                Console.WriteLine($"Requesting {symbol}");
                priceSet = _source.DownloadData(symbol, _dateRange, _interval);
                Console.WriteLine(priceSet.Prices[0].High);
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
                repo.Save(priceSet, _interval);
            }
            else Console.WriteLine($"{symbol} data is NULL");

            Console.WriteLine($"{symbol} imported.");
        }



    }
}
