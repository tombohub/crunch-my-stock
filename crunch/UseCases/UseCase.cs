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
using ScottPlot;
using ScottPlot.Plottable;
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


        /// <summary>
        /// Calculate the count of winners and losers
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Winners and losers count data</returns>
        private static List<WinnersLosersReport> CalculateWinnersLosers(List<WeeklyOvernightStat> stats)
        {
            var winnersCount = stats.Where(s => s.ReturnOnInitialCapital >= 0)
                .Where(s => s.SecurityType == "stocks")
                .Where(s => s.Strategy == "overnight")
                .Count();
            var losersCount = stats.Where(s => s.ReturnOnInitialCapital < 0)
                .Where(s => s.SecurityType == "stocks")
                .Where(s => s.Strategy == "overnight")
                .Count();

            var reportData = new List<WinnersLosersReport>
            {
                new WinnersLosersReport { Type = "Winners", Count = winnersCount },
                new WinnersLosersReport { Type = "Losers", Count = losersCount }
            };
            return reportData;
        }

        /// <summary>
        /// Calculate Top 10 securities by ROI for overnight strategy
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Top 10 report data</returns>
        // HACK: repeated weekNum parameter
        public static List<Top10Report> CalculateTop10(List<WeeklyOvernightStat> stats)
        {
            // sort top 10 from database
            List<WeeklyOvernightStat> top10 = stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == "overnight")
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

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }
            
            return reportData;
        }

        /// <summary>
        /// Calculate bottom 10 securities by Overnight ROI
        /// </summary>
        /// <param name="weekNum">Calendar week number</param>
        /// <returns>Report data for each symbol</returns>
        // HACK: repeated weekNum parameter
        public static List<Bottom10Report> CalculateBottom10(List<WeeklyOvernightStat> stats)
        {
            // sort top 10 from database
            List<WeeklyOvernightStat> bottom10 = stats
                .Where(s => s.SecurityType == "stocks") // HACK: magic string
                .Where(s => s.Strategy == "overnight")
                .OrderBy(s => s.ReturnOnInitialCapital)
                .Take(10)
                .ToList();

            var reportData = new List<Bottom10Report>();
            foreach (var item in bottom10)
            {
                double benchmarkRoi = stats
                    .Where(s => s.Symbol == item.Symbol)
                    .Where(s => s.Strategy == "benchmark")
                    .Select(s => s.ReturnOnInitialCapital)
                    .Single();

                reportData.Add(new Bottom10Report
                {
                    Symbol = item.Symbol,
                    StrategyRoi = item.ReturnOnInitialCapital,
                    BenchmarkRoi = benchmarkRoi
                });

                Console.WriteLine($"{item.Symbol} : {benchmarkRoi}");
            }
            return reportData;
        }


        private static double CalculateAverageOvernightRoi(List<WeeklyOvernightStat> stats)
        {
            double averageRoi = stats
                .Where(s => s.Strategy == "overnight") // HACK: miagic string
                .Where(s => s.SecurityType == "stocks") //hack: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Average();

            return averageRoi;
        }

        private static double CalculateAverageBenchmarkRoi(List<WeeklyOvernightStat> stats)
        {
            double averageBenchmarkRoi = stats
                .Where(s => s.Strategy == "benchmark") //hack: magic string
                .Where(s => s.SecurityType == "stocks") //hack: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Average();
            return averageBenchmarkRoi;
        }

        private static double GetSpyBenchmarkRoi(List<WeeklyOvernightStat> stats)
        {
            double spyRoi = stats
                .Where(s => s.Symbol == "SPY") //HACK: magic string
                .Where(s => s.Strategy == "benchmark") //HACK: magic string
                .Select(s => s.ReturnOnInitialCapital)
                .Single();

            return spyRoi;
        }
        private static double GetSpyOvernightRoi(List<WeeklyOvernightStat> stats)
        {
            double spyOvernightRoi = stats
                .Where(s => s.Symbol == "SPY")
                .Where(s => s.Strategy == "overnight")
                .Select(s => s.ReturnOnInitialCapital)
                .Single();

            return spyOvernightRoi;
        }



        /// <summary>
        /// Plot Winners vs Losers pie chart Overnight strategy
        /// </summary>
        /// <param name="weekNum"></param>
        // HACK: week num as parameter
        public static void PlotWinnersLosers(List<WinnersLosersReport> winLosData)
        {
            var plt = new ScottPlot.Plot();

            double[] values = winLosData.Select(d => (double)d.Count).ToArray();
            string[] labels = winLosData.Select(d => d.Type).ToArray();
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
        public static void PlotTop10(List<Top10Report> top10Data)
        {
            Plot plt = new Plot(600, 400);

            List<Top10Report> orderedTop10 = top10Data.OrderByDescending(t => t.StrategyRoi).ToList();

            // bars overnight roi
            double[] values = orderedTop10
                .Select(t => t.StrategyRoi)
                .ToArray();

            string[] labels = orderedTop10
                .Select(t => t.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, color: Color.DarkGreen);
            bar.Label = "Overnight";
            bar.Orientation = Orientation.Horizontal;

            // lines benchmark roi
            double[] x = orderedTop10
                .Select(t => (double)t.BenchmarkRoi)
                .ToArray();

            double[] y = Enumerable.Range(0, top10Data.Count)
                .Select(y => (double)y)
                .ToArray();

            var benchmarkLines = plt.AddScatterPoints(x, y, Color.Black, 11, MarkerShape.verticalBar, "Buy & Hold");

            plt.Title("Top 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat("P1", false);
            plt.Legend();

            // HACK: dealing with files, magic string
            plt.SaveFig("D:\\PROJEKTI\\top10.png");
        }

        public static void PlotBottom10(List<Bottom10Report> bottom10Data)
        {
            var plt = new Plot(600, 400);

            List<Bottom10Report> orderedBottom10 = bottom10Data.OrderBy(b => b.StrategyRoi).ToList();

            double[] values = orderedBottom10
                .Select(b => b.StrategyRoi)
                .ToArray();

            string[] labels = orderedBottom10
                .Select(b => b.Symbol)
                .ToArray();


            BarPlot bar = plt.AddBar(values, Color.IndianRed);
            bar.FillColorNegative = bar.FillColor;
            bar.Label = "Overnight";
            bar.Orientation = Orientation.Horizontal;

            //lines benchmark ROI
            double[] benchmarkRois = orderedBottom10
                .Select(b => (double)b.BenchmarkRoi)
                .ToArray();

            double[] symbolPositions = Enumerable.Range(0, bottom10Data.Count)
                .Select(s => (double)s)
                .ToArray();

            ScatterPlot benchmarkLines = plt.AddScatterPoints(
                benchmarkRois,
                symbolPositions,
                Color.Black,
                11,
                MarkerShape.verticalBar,
                "Buy & Hold");

            plt.Title("Bottom 10");
            plt.YTicks(labels);
            plt.XLabel("ROI");
            plt.XAxis.TickLabelFormat("P1", false);
            plt.Legend(location: Alignment.UpperLeft);

            // HACK: dealing with files, magic string
            plt.SaveFig("D:\\PROJEKTI\\bot10.png");
        }

        private static void DrawRoiBox(string text, string filename)
        {
            // initialize objects
            Bitmap png = new Bitmap(200, 200);
            Graphics gra = Graphics.FromImage(png);

            // make white background
            gra.FillRectangle(Brushes.White, 0, 0, png.Width, png.Height);

            // center text
            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            var rect = new Rectangle(0, 0, 200, 200);

            gra.DrawString(text, new Font("Verdana", 18), Brushes.Black, rect, format);

            png.Save($"D:\\PROJEKTI\\{filename}.png");
        }

        /// <summary>
        /// Draws the Spy benchmark ROI
        /// </summary>
        public static void DrawSpyBenchmarkRoi(double spyRoi)
        {
            string text = $"SPY\n{spyRoi:P2}";
            DrawRoiBox(text, "spybenchroi");
        }
        public static void DrawSpyOvernightRoi(double spyRoi)
        {
            string text = $"SPY Overnight\n{spyRoi:P2}";
            DrawRoiBox(text, "spyovernightroi");
        }

        public static void DrawAverageOvernightRoi(double averageRoi)
        {
            string text = $"Average ROI\n{averageRoi:P2}";
            DrawRoiBox(text, "avgroi");
        }

        public static void DrawAverageBenchmarkRoi(double averageRoi)
        {
            string text = $"Buy Hold ROI\n{averageRoi:P2}";
            DrawRoiBox(text, "buyholdroi");
        }

        #region composition usecase
        public static void PlotWinnersLosersUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<WinnersLosersReport> winLosData = CalculateWinnersLosers(stats);
            PlotWinnersLosers(winLosData);
        }

        public static void PlotTop10UseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<Top10Report> top10Data = CalculateTop10(stats);
            PlotTop10(top10Data);
        }

        public static void PlotBottom10UseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            List<Bottom10Report> bottom10Data = CalculateBottom10(stats);
            PlotBottom10(bottom10Data);
        }

        public static void DrawSpyBenchmarkRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double spyRoi = GetSpyBenchmarkRoi(stats);
            DrawSpyBenchmarkRoi(spyRoi);
        }

        public static void DrawSpyOvernightRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double spyRoi = GetSpyOvernightRoi(stats);
            DrawSpyOvernightRoi(spyRoi);
        }

        public static void DrawAverageOvernightRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double avgRoi = CalculateAverageOvernightRoi(stats);
            DrawAverageOvernightRoi(avgRoi);
        }

        public static void DrawAverageBenchmarkRoiUseCase(int weekNum)
        {
            List<WeeklyOvernightStat> stats = DatabaseAPI.GetWeeklyOvernightStats(weekNum);
            double avgRoi = CalculateAverageBenchmarkRoi(stats);
            DrawAverageBenchmarkRoi(avgRoi);
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
