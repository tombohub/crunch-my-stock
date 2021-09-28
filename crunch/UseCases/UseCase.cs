using Crunch.Domain;
using Crunch.Database.Models;
using Crunch.Database;
using Crunch.DataSources;
using Crunch.Strategies.Overnight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.DataSources.Fmp.Endpoints;
using Microsoft.EntityFrameworkCore;

using System.Drawing;

namespace Crunch.UseCases
{
    class UseCase
    {
        #region weekly overnight
        /// <summary>
        /// Import prices needed for Weekly Overnight strategy into the database
        /// </summary>
        /// <param name="weekNum">calendar week number</param>
        public static void ImportPricesForOvernight(int weekNum)
        {
            // initialize
            var options = new PriceDownloadOptions(weekNum);
            var fmp = new FmpDataSource();
            List<Database.Models.Security> securities;

            using (var db = new stock_analyticsContext())
            {
                securities = db.Securities.ToList();
                foreach (var security in securities)
                {
                    Console.WriteLine($"Downloading {security.Symbol}");
                    try
                    {
                        var prices = fmp.GetPrices(security.Symbol, options.Interval,
                                                   options.Start,
                                                   options.End);
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
                            var dbPrice = new Price
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
                        Console.WriteLine(e.StackTrace);
                    }

                    Console.WriteLine($"Symbol {security.Symbol} saved");
                }
            }
        }

        







        #region composition usecase
        public static void PlotWinnersLosersUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<WinnersLosersReport> winLosData = Reports.CalculateWinnersLosersRatio(stats);
            Plot.PlotWinnersLosers(winLosData);
        }

        public static void PlotTop10UseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<Top10Report> top10Data = Reports.CalculateTop10(stats);
            Plot.PlotTop10(top10Data);
        }

        public static void PlotBottom10UseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<Bottom10Report> bottom10Data = Reports.CalculateBottom10(stats);
            Plot.PlotBottom10(bottom10Data);
        }

        public static void DrawSpyBenchmarkRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double spyRoi = Reports.GetSpyBenchmarkRoi(stats);
            Plot.DrawSpyBenchmarkRoi(spyRoi);
        }

        public static void DrawSpyOvernightRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double spyRoi = Reports.GetSpyOvernightRoi(stats);
            Plot.DrawSpyOvernightRoi(spyRoi);
        }

        public static void DrawAverageOvernightRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double avgRoi = Reports.CalculateAverageOvernightRoi(stats);
            Plot.DrawAverageOvernightRoi(avgRoi);
        }

        public static void DrawAverageBenchmarkRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double avgRoi = Reports.CalculateAverageBenchmarkRoi(stats);
            Plot.DrawAverageBenchmarkRoi(avgRoi);
        }

        #endregion composition usecase

        #endregion weekly overnight


        #region securities operations
        /// <summary>
        /// Update the list of securities in database
        /// </summary>
        public static void UpdateSecurities()
        {
            #region data source
            // instantiate sources
            var stocksSource = new StocksListEndpoint();
            var etfSource = new EtfListEndpoint();
            var symbolSource = new TradableSymbolsListEndpoint();
            // get symbols
            var updatedStocks = stocksSource.GetStocks();
            var updatedEtfs = etfSource.GetEtfs();
            var symbols = symbolSource.GetSymbols();
            // filter symbols from result
            var newStockSymbols = updatedStocks
                .Where(s => s.Symbol.Length <= 4)
                .Select(s => s.Symbol).ToList();
            var newEtfSymbols = updatedEtfs
                .Where(s => s.Symbol.Length <= 4)
                .Select(s => s.Symbol).ToList();
            var newTradablSymbols = symbols
                .Where(s => s.Symbol.Length <= 4)
                .Select(s => s.Symbol).ToList();
            // filter separate new stocks and new Etfs
            var newTradableStockSymbols = newTradablSymbols.Intersect(newStockSymbols).ToList();
            var newTradableEtfSymbols = newTradablSymbols.Intersect(newEtfSymbols).ToList();
            #endregion 

            #region database
            // initialize database context
            var db = new stock_analyticsContext();
            // truncate the table
            db.Database.ExecuteSqlRaw("TRUNCATE TABLE securities");

            // insert symbols
            foreach (var symbol in newTradableStockSymbols)
            {
                var security = new Crunch.Database.Models.Security
                {
                    Symbol = symbol,
                    Type = "stocks"
                };
                db.Securities.Add(security);
                Console.Write("s");
            }
            foreach (var symbol in newTradableEtfSymbols)
            {
                var security = new Crunch.Database.Models.Security
                {
                    Symbol = symbol,
                    Type = "etfs"
                };
                db.Securities.Add(security);
                Console.Write('e');
            }
            db.SaveChanges();
            db.Dispose();
            #endregion


        }

        #endregion securities operations
    }
}
