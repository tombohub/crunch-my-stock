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

        
        public void Save(PriceSet priceSet)
        {
            string interval = priceSet.Interval switch
            {
                PriceInterval.OneDay => "1d",
                PriceInterval.ThirtyMinutes => "30m",
                _ => throw new ArgumentException("Interval doesn't exist")
            };

            foreach (var price in priceSet.Prices)
            {
                var priceDb = new Models.Price
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

                _db.Prices.Add(priceDb);
            }
            _db.SaveChanges();
        }
    }
}
