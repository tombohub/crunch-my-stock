using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Crunch.Core;
using Crunch.Database;
using CrunchImport.DataProviders;

namespace Crunch
{
    internal static class ApplicationService
    {
        private static DataProviderService _dataProvider = new DataProviderService();

        /// <summary>
        /// Import today's pricesToday for all securities into database
        /// </summary>
        public static void ImportPrices(DateOnly date)
        {
            // trading day value object throws exception if not trading day
            var tradingDay = new TradingDay(date);

            // get symbols from database
            List<Symbol> symbols = Helpers.GetSecuritySymbols();

            // loop over each symbol, get price for the day and save into database.
            // One OHLC price - one save to database
            foreach (var symbol in symbols)
            {
                Thread.Sleep(300);
                var thread = new Thread(() => ImportSymbolPrice(symbol, tradingDay));
                thread.Start();
            }
        }

        /// <summary>
        /// Imports price for a symbol from a data source into the database.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="date">Price date</param>
        private static void ImportSymbolPrice(Symbol symbol, TradingDay tradingDay)
        {
            try
            {
                Console.WriteLine($"Importing pricesToday for {symbol}...");
                SecurityPrice symbolPrice = _dataProvider.GetDailyPrice(symbol, tradingDay);

                Helpers.SaveDailyPrice(symbolPrice);
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
        public static void UpdateSecurities()
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
                        Helpers.SaveSecurity(security);
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

        public static void Analyze(DateOnly date)
        {
            var tradingDay = new TradingDay(date);
            var prevTradingDay = tradingDay.FindPreviousTradingDay();

            // get prices from database for today and prev trading day
            var pricesToday = Helpers.GetPrices(tradingDay)
                .Select(price => MapSecurityPriceDtoToSecurityPrice(price))
                .ToList();

            var pricesPrevDay = Helpers.GetPrices(prevTradingDay)
                .Select(price => MapSecurityPriceDtoToSecurityPrice(price))
                .ToList();

            var domainService = new DomainService();
            var pricesOvernight = domainService.TransformToOvernightPrices(pricesPrevDay, pricesToday);

            Console.WriteLine(pricesToday[0].Symbol);
            // perform calculations
            Analytics.WinnersLosers(pricesToday);
            // save result to database
            Helpers.SaveWinnersLosers();
        }

        private static SecurityPrice MapSecurityPriceDtoToSecurityPrice(SecurityPriceDTO pricesDto)
        {
            var securityPrice = new SecurityPrice
            {
                Symbol = new Symbol(pricesDto.Symbol),
                Date = new TradingDay(pricesDto.Date),
                Price = new OHLC(open: pricesDto.Open, high: pricesDto.High, low: pricesDto.Low, close: pricesDto.Close),
                Volume = pricesDto.Volume,
            };
            return securityPrice;
        }
    }
}