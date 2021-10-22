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
using Crunch.Strategies.Overnight.Reports;
using Crunch.Strategies.Overnight.Plots;

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
                            string interval = price.interval switch
                            {
                                PriceInterval.OneDay => "1d",
                                PriceInterval.ThirtyMinutes => "30m",
                                _ => throw new ArgumentException("Interval doesn't exist"),
                            };
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
        public static void PlotWinnersLosersUseCase(int weekNum, SecurityType securityType) 
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);
            WinnersLosersRatioReport winLosData = overnightStats.CalculateWinnersLosersRatio(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.PlotWinnersLosers(winLosData, 300, 300);
        }

        public static void PlotTop10UseCase(int weekNum, SecurityType securityType)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            List<Top10Report> top10Data = overnightStats.CalculateTop10(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.PlotTop10(top10Data, 300, 300);
        }

        public static void PlotBottom10UseCase(int weekNum, SecurityType securityType)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            List<Bottom10Report> bottom10Data = overnightStats.CalculateBottom10(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.PlotBottom10(bottom10Data, 300, 300);
        }

        public static void DrawSpyBenchmarkRoiUseCase(int weekNum)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            double spyRoi = overnightStats.GetSpyBenchmarkRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawSpyBenchmarkRoi(spyRoi, 300, 300);
        }

        public static void DrawSpyOvernightRoiUseCase(int weekNum)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            double spyRoi = overnightStats.GetSpyOvernightRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawSpyOvernightRoi(spyRoi, 300, 300);
        }

        public static void DrawAverageOvernightRoiUseCase(int weekNum, SecurityType securityType)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            double avgRoi = overnightStats.CalculateAverageOvernightRoi(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawAverageOvernightRoi(avgRoi, 300, 300);
        }

        public static void DrawAverageBenchmarkRoiUseCase(int weekNum)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            double avgRoi = overnightStats.CalculateAverageBenchmarkRoi();
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.DrawAverageBenchmarkRoi(avgRoi, 300, 300);
        }

        public static void PlotOvernightUseCase(int weekNum, SecurityType securityType)
        {
            var repo = new OvernightStatsRepository();
            var overnightStats = repo.GetOvernightStats(weekNum);

            ReportsCollection reports = overnightStats.CreateReports(securityType);
            OvernightPlotter overnightMultiplot = new();
            overnightMultiplot.CreateMultiplot(reports);
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
