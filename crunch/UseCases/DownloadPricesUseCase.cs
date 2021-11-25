using Crunch.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crunch.DataSources;
using Crunch.Database;
using Crunch.Domain;
using Crunch.Domain.OhlcPrice;
using System.Net;

namespace Crunch.UseCases
{
    
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

        /// <summary>
        /// Price data interval
        /// </summary>
        private readonly PriceInterval _interval;

        /// <summary>
        /// Pause between data source API request in miliseconds. 
        /// To avoid 'too many requests' error
        /// </summary>
        private readonly int _requestPause = 210;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Prices data starting date</param>
        /// <param name="end">Prices data ending date, inclusive</param>
        /// <param name="interval">Prices data interval between each price</param>
        public DownloadPricesUseCase(DateOnly start, DateOnly end, PriceInterval interval)
        {
            _startDate = start;
            _endDate = end;
            _interval = interval;
            ValidateDates(start, end);
        }
        public void Execute()
        {
            // call data source interface
            // fetch data
            // save to database
            List<string> symbols = Helpers.GetSecuritySymbols();
            Helpers.TruncatePricesTable();

            foreach (string symbol in symbols)
            {
                Console.WriteLine($"Creating task for {symbol}");
                var thread = new Thread(() => ImportPrices(symbol));
                Thread.Sleep(_requestPause);
                thread.Start();
            }
        }

        private void ImportPrices(string symbol)
        {
            PriceSet priceSet = null;

            try
            {
                Console.WriteLine($"Requesting {symbol}");
                priceSet = _source.DownloadData(symbol, _startDate, _endDate, _interval);
                Console.WriteLine(priceSet.Prices[0].High);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (priceSet != null)
            {
                Console.WriteLine($"SAving {symbol}");

                //HACK: creating new instance of repo to make it thread safe
                PriceSetRepository repo = new PriceSetRepository();
                repo.Save(priceSet);
            }
            else Console.WriteLine($"{symbol} data is NULL");

            Console.WriteLine($"{symbol} imported.");
        }


        /// <summary>
        /// Checks if starting date is later than ending date.
        /// Throws exception if true.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateDates(DateOnly start, DateOnly end)
        {
            if (start > end) throw new ArgumentException("Starting date cannot be later than ending date");
        }
    }
}
