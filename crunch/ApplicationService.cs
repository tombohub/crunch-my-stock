﻿using System;
using System.Collections.Generic;
using System.Threading;
using Crunch.Core;
using Crunch.Database;
using CrunchImport.DataProviders;

namespace Crunch
{
    internal class ApplicationService
    {
        private readonly DatabaseMethods _db = new DatabaseMethods();
        private readonly DataProviderService _dataProvider = new DataProviderService();

        /// <summary>
        /// Import today's pricesToday for all securities into database
        /// </summary>
        public void ImportPrices(DateOnly date)
        {
            // trading day value object throws exception if not trading day
            var tradingDay = new TradingDay(date);

            // get symbols from database
            var db = new DatabaseMethods();
            List<Symbol> symbols = db.GetSecuritySymbols();

            // loop over each symbol, get price for the day and save into database.
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Thread.Sleep(300);
                var thread = new Thread(() => ImportSymbolPrice(symbol, tradingDay));
                thread.Start();
            }
        }

        public void ImportOvernightPrices(DateOnly date)
        {
            var tradingDay = new TradingDay(date);
            var prevTradingDay = tradingDay.FindPreviousTradingDay();

            var db = new DatabaseMethods();
            var todayPrices = db.GetPrices(tradingDay);
            var prevTradDayPrices = db.GetPrices(prevTradingDay);

            var domainService = new DomainService();
            var overnightPrices = domainService.TransformToOvernightPrices(prevTradDayPrices, todayPrices);

            db.SaveOvernightPrices(overnightPrices);
        }

        /// <summary>
        /// Imports price for a symbol from a data source into the database.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="date">Price date</param>
        private void ImportSymbolPrice(Symbol symbol, TradingDay tradingDay)
        {
            try
            {
                Console.WriteLine($"Importing pricesToday for {symbol}...");
                SecurityPrice symbolPrice = _dataProvider.GetDailyPrice(symbol, tradingDay);

                _db.SaveDailyPrice(symbolPrice);
                Console.WriteLine($"Prices imported for {symbol}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message} with symbol {symbol}");
            }
        }

        /// <summary>
        /// Update securities in database
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
            List<SecurityPriceOvernight> pricesOvernight = domainService.TransformToOvernightPrices(pricesPrevDay, pricesToday);

            // perform calculations
            var analytics = new AnalyticMethods();
            WinnersLosersCount winnersLosers = analytics.WinnersLosers(pricesOvernight, SecurityType.Stock);

            decimal avgRoi = analytics.AverageRoi(pricesOvernight);
            decimal spyRoi = analytics.AverageSpyRoi(pricesOvernight);
            Console.WriteLine(spyRoi);
            //save result to database
            _db.SaveWinnersLosers(winnersLosers);
        }
    }
}