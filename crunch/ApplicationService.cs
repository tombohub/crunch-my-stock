using Crunch.Core;
using Crunch.Database;
using Crunch.DataProviders;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Crunch
{
    /// <summary>
    /// Use cases of application.
    /// Central point which combines outside infrastructure with domain
    /// </summary>
    internal class ApplicationService
    {
        private readonly DatabaseMethods _db = new DatabaseMethods();
        private readonly DataProviderService _dataProvider = new DataProviderService();



        /// <summary>
        /// Saves overnight prices to database.
        /// 
        /// Overnight prices are saved into separate table because it's
        /// easier to do analytics that way.
        /// </summary>
        /// <param name="date">Date when the market opens</param>
        public void SaveOvernightPrices(DateOnly date)
        {
            var tradingDay = new TradingDay(date);
            var prevTradingDay = tradingDay.FindPreviousTradingDay();

            var db = new DatabaseMethods();
            var todayPrices = db.GetPrices(tradingDay);
            var prevTradDayPrices = db.GetPrices(prevTradingDay);

            var domainService = new DomainService();
            var overnightPrices = domainService.TransformToOvernightPrices(prevTradDayPrices, todayPrices);

            db.SaveOvernightPrices(overnightPrices.Prices);
        }



        /// <summary>
        /// Update securities in database.
        /// 
        /// Inserts new securities, updates listed delisted status
        /// </summary>
        public void UpdateSecurities()
        {
            Console.WriteLine("Updating securities...");

            List<SecurityDTO> securitiesDto = _dataProvider.GetSecurities();

            // map dto to entity
            var securities = new List<Security>();
            foreach (var security in securitiesDto)
            {
                try
                {
                    securities.Add(new Security
                    {
                        Symbol = new Symbol(security.Symbol),
                        Exchange = security.Exchange,
                        Status = security.Status,
                        Type = security.Type,
                        IpoDate = security.IpoDate,
                        DelistingDate = security.DelistingDate
                    });
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }

            // import to database
            foreach (var security in securities)
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        Console.WriteLine($"Updating {security.Symbol.Value}...");
                        _db.SaveSecurity(security);
                        Console.WriteLine($"{security.Symbol.Value} updated.");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
                thread.Start();
            }
        }

        /// <summary>
        /// Calculates stats for overnight strategy
        /// </summary>
        /// <param name="date"></param>
        public void Analyze(DateOnly date)
        {
            var tradingDay = new TradingDay(date);
            var prevTradingDay = tradingDay.FindPreviousTradingDay();

            // get prices from database for today
            var pricesToday = _db.GetPrices(tradingDay);

            // get prices for previous trading day
            var pricesPrevDay = _db.GetPrices(prevTradingDay);

            // make overnight prices out of today and prev trad day
            var domainService = new DomainService();
            DailyPricesOvernight pricesOvernight = domainService.TransformToOvernightPrices(pricesPrevDay, pricesToday);

            // perform calculations
            var analytics = new AnalyticMethods(pricesOvernight);
            List<WinnersLosersCount> winnersLosers = analytics.WinnersLosers();
            List<AverageRoi> averageRoi = analytics.AverageRoi();
            SpyRoi spyRoi = analytics.AverageSpyRoi();


        }

        /// <summary>
        /// Creates plot out of the stats for overnight strategy
        /// </summary>
        /// <param name="date"></param>
        public void Plot(DateOnly date, SecurityType securityType)
        {
            // get data from database for each report
            // create individual plot
            // get coordinates for each plot to add in multiplot
            // create multiplot using individual plots and coordinates
            // save as image file
            //IStrategyService strategyService = StrategyServiceFactory.CreateService(strategy);
            //strategyService.CreateStrategyMultiplot(date);
            var winnersLosers = _db.GetWinnersLosers(new TradingDay(date), securityType);
            var plots = new PlottingMethods();
            var plotImage = plots.WinnersLosersPlot(winnersLosers);

            plotImage.Save("C:/Users/Shmukaluka/Downloads/kloko.png");
        }
    }
}