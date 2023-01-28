using System;
using Crunch.Core;
using Crunch.Database.Models;

namespace Crunch.Database
{
    internal class IntradayPriceSetRepository
    {
        /// <summary>
        /// Database context object
        /// </summary>
        private stock_analyticsContext _db;

        /// <summary>
        /// Save PriceSet object to the database
        /// </summary>
        /// <param name="priceSet"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Save(IntradayPriceSet priceSet, PriceInterval interval)
        {
            string intervalDb = Helpers.PriceIntervalToString(interval);
            using (_db = new stock_analyticsContext())
            {
                foreach (var price in priceSet.Prices)
                {
                    var priceDb = new Models.PricesIntraday
                    {
                        Symbol = priceSet.Symbol,
                        Open = price.Open,
                        High = price.High,
                        Low = price.Low,
                        Close = price.Close,
                        Volume = price.Volume,
                        Timestamp = price.Timestamp,
                        Interval = intervalDb
                    };
                    //TODO: manage intraday and daily prices separatelly
                    _db.PricesIntradays.Add(priceDb);
                }
                _db.SaveChanges();
            }
        }
    }
}