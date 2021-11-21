using Crunch.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        PriceDataSource _priceDataSource = new PriceDataSource();

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
            //Helpers.TruncatePricesTable();
            foreach (string symbol in symbols)
            {
                try
                {
                    PriceSet priceSet = _priceDataSource.DownloadData(symbol, _startDate, _endDate, _interval);
                    Console.WriteLine(priceSet.Symbol);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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
