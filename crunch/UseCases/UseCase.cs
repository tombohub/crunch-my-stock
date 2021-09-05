using Crunch.Core;
using Crunch.Database.Models;
using Crunch.DataSource;
using Crunch.Strategies.Overnight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.UseCases
{
    class UseCase
    {
        public static void ImportPricesForOvernight(int weekNum)
        {
            var options = new PriceDownloadOptions(weekNum);
            var fmp = new Fmp();
            List<Security> securities;

            using (var db = new stock_analyticsContext())
            {
                securities = db.Securities.ToList();
                foreach (var security in securities)
                {
                    Console.WriteLine($"Downloading {security.Symbol}");
                    try
                    {
                        var prices = fmp.GetPrices(security.Symbol, options.Interval,
                                                   options.Start.ToString("yyyy-MM-dd"),
                                                   options.End.ToString("yyyy-MM-dd"));
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

                    Console.WriteLine($"Symbol {security.Symbol} saved");


                }
            }
        }
    }
}
