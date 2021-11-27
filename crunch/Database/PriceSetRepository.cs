using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.Domain.OhlcPrice;
using Crunch.Database.Models;

namespace Crunch.Database
{
    internal class PriceSetRepository
    {
        /// <summary>
        /// Database context object
        /// </summary>
        private stock_analyticsContext _db = new stock_analyticsContext();

        
        /// <summary>
        /// Save PriceSet object to the database
        /// </summary>
        /// <param name="priceSet"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Save(PriceSet priceSet, PriceInterval interval)
        {
            switch (interval)
            {
                case PriceInterval.OneDay:
                    SaveDayPrices(priceSet, "1d");
                    break;
                case PriceInterval.ThirtyMinutes:
                    SaveIntradayPrices(priceSet, "30m");
                    break ;
                default:
                    throw new ArgumentException("Interval doesn't exist", nameof(interval));
            }
        }

        private void SaveDayPrices(PriceSet priceSet, string interval)
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
                    Interval = interval
                };
                //TODO: manage intraday and daily prices separatelly 
                _db.PricesDailies.Add(priceDb);
            }
            _db.SaveChanges();
        }

        private void SaveIntradayPrices(PriceSet priceSet, string interval)
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
                    Interval = interval
                };
                //TODO: manage intraday and daily prices separatelly 
                _db.PricesIntradays.Add(priceDb);
            }
            _db.SaveChanges();
        }

        public void Load(string symbol, PriceInterval interval, DateOnly start,  DateOnly end)
        {

        }
    }
}
