using Crunch.Database.Models;
using Crunch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Crunch.Database
{
    internal class DailyPriceSetRepository
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
        public void Save(DailyPriceSet priceSet, PriceInterval interval)
        {
            string intervalDb = Helpers.PriceIntervalToString(interval);
            using (_db = new stock_analyticsContext())
            {
                foreach (var price in priceSet.Prices)
                {
                    var priceDb = new Models.PricesDaily
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
                    _db.PricesDailies.Add(priceDb);
                }
                _db.SaveChanges();
            }
        }

        // TODO: unfinished method for inserting into prices_daily using Dapper
        public void Save(DailyPriceSet priceSet, PriceInterval interval, string db)
        {
            using var conn = DbConnections.CreatePsqlConnection();
            
        }



        //public DailyPriceSet Load(string symbol, PriceInterval interval, DateOnly start, DateOnly end)
        //{
        //    string intervalDb = Helpers.PriceIntervalToString(interval);

        //    var pricesDb = new List<PricesDaily>();
        //    using (_db = new stock_analyticsContext())
        //    {
        //        pricesDb = _db.PricesDailies
        //            .Where(price => price.Symbol == symbol)
        //            .Where(price => price.Interval == intervalDb)
        //            .Where(price => (price.Timestamp >= start) && (price.Timestamp <= end))
        //            .ToList();
        //    }

        //    Console.WriteLine(pricesDb);

        //    var priceSet = new List<PriceDaily>();
        //    foreach (var priceDb in pricesDb)
        //    {
        //        var price = new PriceDaily(
        //            Timestamp: priceDb.Timestamp,
        //            Open: priceDb.Open,
        //            High: priceDb.High,
        //            Low: priceDb.Low,
        //            Close: priceDb.Close,
        //            Volume: priceDb.Volume);
        //        priceSet.Add(price);
        //    }

        //    return new DailyPriceSet(
        //        Symbol: symbol,
        //        Interval: interval,
        //        Prices: priceSet);
        //}
    }
}
