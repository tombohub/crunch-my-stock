using Crunch.Core;
using Crunch.Database.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    public class DatabaseMethods
    {
        /// <summary>
        /// Entity framework database context
        /// </summary>
        private readonly stock_analyticsContext _db = new stock_analyticsContext();

        /// <summary>
        /// Get list of all symbols from database
        /// </summary>
        /// <returns></returns>
        public List<Core.Security> GetSecurities()
        {
            var symbols = _db.Securities
                .Where(x => x.Status == SecurityStatus.Active.ToString())
                .Select(s => new Core.Security
                {
                    Exchange = Enum.Parse<Exchange>(s.Exchange),
                    IpoDate = s.IpoDate,
                    Status = Enum.Parse<SecurityStatus>(s.Status),
                    Type = Enum.Parse<SecurityType>(s.Type),
                    Symbol = new Symbol(s.Symbol),
                }).ToList();
            return symbols;
        }

        /// <summary>
        /// Get list of Symbols by security type
        /// </summary>
        /// <param name="securityType"></param>
        /// <returns></returns>
        public List<Symbol> GetSecuritySymbols(SecurityType securityType)
        {
            var securites = _db.Securities
                .Where(x => x.Type == securityType.ToString())
                .Select(x => new Symbol(x.Symbol))
                .ToList();

            return securites;
        }

        /// <summary>
        /// Get multiplot image size in pixels for the given strategy.
        /// Uses SQL to calculate the size.
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public Size GetMultiplotSize(StrategyName strategy)
        {
            var dbConnection = DbConnections.CreatePsqlConnection();
            var sql = @"SELECT  max((x+width)*scale) as width, max((y+height)*scale) as height
                        FROM public.multiplot_coordinates
                        WHERE strategy = 'Overnight' and is_included = true";
            var parameters = new { Strategy = strategy.ToString() };
            var multiplotSize = dbConnection.QuerySingle<Size>(sql, parameters);
            return multiplotSize;
        }

        /// <summary>
        /// Save daily price pricesDto to database. If price for that day/symbol exists
        /// then it will be updated.
        /// </summary>
        /// <param name="price"></param>
        public void SaveDailyPrice(SecurityPrice price)
        {
            var security = _db.Securities
                .Where(x => x.Symbol == price.Symbol.Value)
                .Single();

            var priceDb = new PricesDaily
            {
                Date = price.TradingDay.Date,
                Symbol = price.Symbol.Value,
                SecurityId = security.Id,
                Open = price.OHLC.Open,
                High = price.OHLC.High,
                Low = price.OHLC.Low,
                Close = price.OHLC.Close,
                Volume = price.Volume,
            };
            _db.PricesDailies
                .Upsert(priceDb)
               .On(x => new { x.SecurityId, x.Date })
               .WhenMatched(x => new PricesDaily
               {
                   Open = price.OHLC.Open,
                   High = price.OHLC.High,
                   Low = price.OHLC.Low,
                   Close = price.OHLC.Close
               })
               .Run();
        }

        public void SaveDailyPrice(List<SecurityPrice> prices)
        {
            foreach (SecurityPrice price in prices)
            {
                SaveDailyPrice(price);
            }
        }


        /// <summary>
        /// Save security to database. If security exists in database it will be updated.
        /// </summary>
        /// <param name="security"></param>
        public void SaveSecurity(Core.Security security)
        {
            string sql = $@"INSERT INTO public.securities (symbol, type, exchange, updated_at, status, ipo_date, delisting_date)
                            VALUES(
                                    @Symbol,
                                    @Type,
                                    @Exchange,
                                    @UpdatedAt,
                                    @Status,
                                    @IpoDate,
                                    @DelistingDate
                                    )
                            ON CONFLICT ON CONSTRAINT securities_symbol_un
                            DO UPDATE SET type = @Type,
                                           exchange = @Exchange,
                                           updated_at = @UpdatedAt,
                                            status = @Status,
                                            ipo_date = @IpoDate,
                                            delisting_date = @DelistingDate";
            var parameters = new
            {
                Symbol = security.Symbol.Value,
                Type = security.Type.ToString(),
                Exchange = security.Exchange.ToString(),
                UpdatedAt = DateTime.UtcNow,
                Status = security.Status.ToString(),
                IpoDate = security.IpoDate,
                DelistingDate = security.DelistingDate
            };
            using var conn = DbConnections.CreatePsqlConnection();
            conn.Execute(sql, parameters);
        }

        /// <summary>
        /// Get all pricesDb for overnight strategy
        /// </summary>
        /// <param name="tradingDay"></param>
        /// <returns></returns>
        public DailyPricesRegular GetPrices(TradingDay tradingDay)
        {
            // get today pricesDb
            var pricesDb = _db.PricesDailies
                .Include(x => x.Security)
                .Where(x => x.Date == tradingDay.Date)
                .ToList();

            var securityPrices = new List<SecurityPrice>();
            foreach (var price in pricesDb)
            {
                Symbol symbol = new Symbol(price.Symbol);
                SecurityType securityType = Enum.Parse<SecurityType>(price.Security.Type);
                OHLC ohlc = new OHLC(price.Open, price.High, price.Low, price.Close);

                securityPrices.Add(new SecurityPrice
                {
                    TradingDay = tradingDay,
                    Symbol = symbol,
                    SecurityType = securityType,
                    OHLC = ohlc,
                    Volume = (int)price.Volume
                });
            };

            var dailyPrices = new DailyPricesRegular
            {
                TradingDay = tradingDay,
                SecurityPrices = securityPrices
            };
            return dailyPrices;
        }

        /// <summary>
        /// Save Winners Losers stats to database
        /// </summary>
        /// <param name="winnersLosers"></param>
        public void SaveWinnersLosers(List<Core.WinnersLosersCount> winnersLosers)
        {
            foreach (var item in winnersLosers)
            {
                var winnersLosersDb = new Models.WinnersLosersCount
                {
                    Date = item.TradingDay.Date,
                    WinnersCount = item.WinnersCount,
                    LosersCount = item.LosersCount,
                    SecurityType = item.SecurityType.ToString(),
                };
                _db.WinnersLosersCounts
                    .Upsert(winnersLosersDb)
                   .On(x => new { x.Date, x.SecurityType })
                   .WhenMatched(x => new Models.WinnersLosersCount
                   {
                       WinnersCount = item.WinnersCount,
                       LosersCount = item.LosersCount
                   })
                   .Run();
            }
        }

        public Core.WinnersLosersCount GetWinnersLosers(TradingDay tradingDay, SecurityType securityType)
        {
            var winnersLosersDb = _db.WinnersLosersCounts
                .Where(x => x.Date == tradingDay.Date)
                .Where(x => x.SecurityType == securityType.ToString())
                .Single();

            var winnersLosers = new Core.WinnersLosersCount
            {
                TradingDay = new TradingDay(winnersLosersDb.Date),
                WinnersCount = winnersLosersDb.WinnersCount,
                LosersCount = winnersLosersDb.LosersCount,
                SecurityType = Enum.Parse<SecurityType>(winnersLosersDb.SecurityType)
            };

            return winnersLosers;
        }


        /// <summary>
        /// Saves overnight prices into the database
        /// </summary>
        /// <param name="overnightPrices"></param>
        public void SaveOvernightPrices(List<SecurityPriceOvernight> overnightPrices)
        {
            foreach (var price in overnightPrices)
            {
                Console.WriteLine($"saving {price.Symbol.Value}");
                var security = _db.Securities.Where(x => x.Symbol == price.Symbol.Value).Single();
                var perfDb = new DailyOvernightPerformance
                {
                    Date = price.TradingDay.Date,
                    Open = price.OHLC.Open,
                    Close = price.OHLC.Close,
                    Security = security,
                    ChangePct = (price.OHLC.Close - price.OHLC.Open) / price.OHLC.Open * 100,
                };
                //_db.DailyOvernightPerformances.Add(perfDb);
                //_db.SaveChanges();

                _db.DailyOvernightPerformances
                    .Upsert(perfDb)
                    .On(x => new { x.SecurityId, x.Date })
                    .Run();
            }
        }
    }
}