using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Crunch.Core;
using Crunch.Database.Models;
using Dapper;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    internal class DatabaseMethods
    {
        /// <summary>
        /// Entity framework database context
        /// </summary>
        private readonly stock_analyticsContext _db = new stock_analyticsContext();

        /// <summary>
        /// Get list of all symbols from database
        /// </summary>
        /// <returns></returns>
        public List<Symbol> GetSecuritySymbols()
        {
            var symbols = _db.Securities.Select(s => new Symbol(s.Symbol)).ToList();
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
                Security = security,
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
               });
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
        public List<SecurityPrice> GetPrices(TradingDay tradingDay)
        {
            // get today pricesDb
            var pricesDb = _db.PricesDailies
                .Where(x => x.Date == tradingDay.Date)
                .ToList();

            var prices = pricesDb.Select(price => MapDbPriceToSecurityPrice(price)).ToList();
            return prices;
        }

        public List<SecurityPriceDTO> GetPrices(TradingDay tradingDay, SecurityType securityType)
        {
            string sql = @"SELECT
	                            pd.date,
	                            pd.symbol,
	                            pd.""open"",
	                            pd.high,
	                            pd.low,
	                            pd.""close"",
	                            pd.volume
                            FROM
	                            public.prices_daily pd
                            JOIN public.securities s
		                            USING(symbol)
                            WHERE
	                            s.""type"" = @SecurityType
                            AND
                                pd.date = @TradingDay";

            var parameters = new
            {
                Date = tradingDay.Date,
                SecurityType = securityType.ToString(),
            };

            using var conn = DbConnections.CreatePsqlConnection();
            var prices = conn.Query<SecurityPriceDTO>(sql, parameters).ToList();

            return prices;
        }

        /// <summary>
        /// Map pricesDb from database to prices domain
        /// </summary>
        /// <param name="pricesDb"></param>
        /// <returns></returns>
        private SecurityPrice MapDbPriceToSecurityPrice(PricesDaily pricesDb)
        {
            var securityPriceDTO = new SecurityPrice
            {
                TradingDay = new TradingDay(pricesDb.Date),
                Symbol = new Symbol(pricesDb.Symbol),
                OHLC = new OHLC(pricesDb.Open, pricesDb.High, pricesDb.Low, pricesDb.Close),
                Volume = (int)pricesDb.Volume
            };
            return securityPriceDTO;
        }

        /// <summary>
        /// Save Winners Losers stats to database
        /// </summary>
        /// <param name="winnersLosers"></param>
        public void SaveWinnersLosers(Core.WinnersLosersCount winnersLosers)
        {
            _db.WinnersLosersCounts.Add(new Models.WinnersLosersCount
            {
                Date = winnersLosers.TradingDay.Date,
                WinnersCount = winnersLosers.WinnersCount,
                LosersCount = winnersLosers.LosersCount,
                SecurityType = winnersLosers.SecurityType.ToString(),
            });
            _db.SaveChanges();
        }

        /// <summary>
        /// Gets overnight security prices from database
        /// </summary>
        /// <param name="tradingDay"></param>
        /// <returns>list of prices domain objects</returns>
        public List<Core.SecurityPriceOvernight> GetOvernightPrices(TradingDay tradingDay)
        {
            var prices = _db.PricesDailyOvernights
                .Where(x => x.Date == tradingDay.Date)
                .Select(x => new SecurityPriceOvernight
                {
                    TradingDay = tradingDay,
                    OHLC = new OHLC(x.Open, x.Close),
                    Symbol = new Symbol(x.Security.Symbol)
                })
                .ToList();

            return prices;
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
                _db.PricesDailyOvernights.Add(new PricesDailyOvernight
                {
                    Date = price.TradingDay.Date,
                    Open = price.OHLC.Open,
                    Close = price.OHLC.Close,
                    Security = security
                });
                _db.SaveChanges();
            }
        }
    }
}