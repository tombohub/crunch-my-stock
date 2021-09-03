using Crunch.Core;
using Crunch.Database.Models;
using Crunch.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.UseCases
{
    class UseCase
    {
        public static void ImportPrices()
        {
            var fmp = new Fmp();
            List<Security> stocks;
            using (var db = new stock_analyticsContext())
            {
                stocks = db.Securities
                    .Where(s => s.Type == "stocks")
                    .ToList();
                foreach (var stock in stocks)
                {
                    Console.WriteLine($"Downloading {stock.Symbol}");
                    try
                    {
                        var prices = fmp.GetPrices(stock.Symbol, PriceInterval.OneDay, "2021-08-09");
                        foreach (var price in prices)
                        {
                            string interval;
                            switch (price.interval)
                            {
                                case PriceInterval.OneDay:
                                    interval = "1d";
                                    break;
                                case PriceInterval.ThirtyMinutes:
                                    interval = "30m";
                                    break;
                                default:
                                    throw new ArgumentException("Interval doesn't exist");
                            }
                            var dbPrice = new Price()
                            {
                                Close = price.close,
                                High = price.high,
                                Interval = interval,
                                Low = price.low,
                                Open = price.open,
                                Symbol = price.symbol,
                                Timestamp = price.timestamp,
                                Volume = (long)price.volume
                            };
                            db.Prices.Add(dbPrice);
                        }
                        db.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }

                    Console.WriteLine($"Symbol {stock.Symbol} saved");


                }
            }
        }
    }
}
