using Crunch.Domain;
using Crunch.Database.Models;
using Crunch.DataSources;
using Crunch.Strategies.Overnight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.DataSources.Fmp.Endpoints;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using System.Drawing;
using Microsoft.Data.Analysis;

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

        /// <summary>
        /// Calculate the count of winners and losers
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Winners and losers count data</returns>
        private static List<WinnersLosersReport> CalculateWinnersLosers(int weekNum)
        {
            #region database
            var db = new stock_analyticsContext();
            var stats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum).ToList();
            #endregion

            #region calculation
            var winnersCount = stats.Where(s => s.ReturnOnInitialCapital >= 0)
                .Where(s => s.SecurityType == "stocks")
                .Where(s => s.Strategy == "overnight")
                .Count();
            var losersCount = stats.Where(s => s.ReturnOnInitialCapital < 0)
                .Where(s => s.SecurityType == "stocks")
                .Where(s => s.Strategy == "overnight")
                .Count();
            #endregion

            #region return
            var reportData = new List<WinnersLosersReport>
            {
                new WinnersLosersReport { Type = "Winners", Count = winnersCount },
                new WinnersLosersReport { Type = "Losers", Count = losersCount }
            };
            return reportData;
            #endregion
        }

        /// <summary>
        /// Calculate Top 10 securities by ROI for overnight strategy
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Top 10 report data</returns>
        // HACK: repeated weekNum parameter
        public static List<Top10Report> CalculateTop10(int weekNum)
        {
            // HACK: repeated code fetching from database
            #region database
            var db = new stock_analyticsContext();

            var stats = db.WeeklyOvernightStats
                .Where(s => s.WeekNum == weekNum).ToList();

            db.Dispose();
            #endregion

            #region calculation
            // sort top 10 from database
            List<WeeklyOvernightStat> top10 = stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .OrderByDescending(s => s.ReturnOnInitialCapital)
                .Take(10)
                .ToList();

            // TODO: decide what object is this one
            var reportData = new List<Top10Report>();
            foreach (var item in top10)
            {
                double benchmarkRoi = stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == "benchmark")
                    .Select(s => s.ReturnOnInitialCapital)
                    .Single();

                reportData.Add(new Top10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.ReturnOnInitialCapital,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine(item.Symbol);
            }
            #endregion

            #region return
            return reportData;
            #endregion

        }

        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="weekNum"></param>
        // HACK: week num as parameter
        public static void PlotWinnersLosers(int weekNum)
        {
            var plt = new ScottPlot.Plot();
            // HACK: dependency on method
            var reportData = UseCase.CalculateWinnersLosers(weekNum);

            double[] values = reportData.Select(d => (double)d.Count).ToArray();
            string[] labels = reportData.Select(d => d.Type).ToArray();
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;
            pie.SliceFillColors = new Color[] { Color.DarkGreen, Color.DarkRed };
            pie.Explode = true;
            plt.SaveFig("D:\\PROJEKTI\\pie.png");
        }

        /// <summary>
        /// Plot Weekly Overnight top 10 ROI
        /// </summary>
        /// <param name="weekNum"></param>
        // HACK: week num as parameter, shouldnt be
        public static void PlotTop10(int weekNum)
        {
            var plt = new ScottPlot.Plot(600, 400);
            // HACK: dependency on calculation method
            List<Top10Report> top10 = UseCase.CalculateTop10(weekNum);

            double[] values = top10
                .OrderBy(t => t.StrategyRoi)
                .Select(t => t.StrategyRoi)
                .ToArray();

            string[] labels = top10
                .OrderBy(t => t.StrategyRoi)
                .Select(t => t.Symbol)
                .ToArray();

            plt.Title("Top 10");

            var bar = plt.AddBar(values);
            bar.Label = "Overnight ROI";
            bar.Orientation = Orientation.Horizontal;
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat("P1", false);
            plt.SetAxisLimits(xMin: 0);

            double[] x = top10
                .Select(t => (double)t.BenchmarkRoi)
                .ToArray();

            double[] y = Enumerable.Range(0, top10.Count)
                .Select(y => (double)y)
                .ToArray();

            var sp = plt.AddScatterPoints(x, y, Color.Black, 11, MarkerShape.verticalBar, "Buy Hold ROI");
            
            plt.Legend();
            
            // HACK: dealing with files, magic string
            plt.SaveFig("D:\\PROJEKTI\\top10.png");
        }
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
