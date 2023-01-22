using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crunch.Database.Models;
using Crunch.Domain;
using Dapper;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Get list of all symbols from database
        /// </summary>
        /// <returns></returns>
        public static List<Symbol> GetSecuritySymbols()
        {
            var db = new stock_analyticsContext();
            var symbols = db.Securities.Select(s => new Symbol(s.Symbol)).ToList();
            return symbols;
        }

        /// <summary>
        /// Get string associated with PriceInterval Enum to use in database
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string PriceIntervalToString(PriceInterval interval)
        {
            string intervalAsString = interval switch
            {
                PriceInterval.OneDay => "1d",
                PriceInterval.ThirtyMinutes => "30m",
                _ => throw new ArgumentException("Interval is not supported", nameof(interval))
            };
            return intervalAsString;
        }

        /// <summary>
        /// Get multiplot image size in pixels for the given strategy.
        /// Uses SQL to calculate the size.
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static Size GetMultiplotSize(StrategyName strategy)
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
        /// Save daily price data to database. If price for that day/symbol exists
        /// then it will be updated.
        /// </summary>
        /// <param name="price"></param>
        public static void SaveDailyPriceAsync(SecurityPrice price)
        {
            string sql = @$"insert into
                 public.prices_daily (date, symbol, open, high, low, close, volume)
                VALUES ('{price.Date.Date}', '{price.Symbol.Value}', '{price.Price.Open}', '{price.Price.High}',
                '{price.Price.Low}', '{price.Price.Close}', '{price.Volume}')
                ON CONFLICT ON CONSTRAINT date_symbol_un
                DO UPDATE SET
                              open = '{price.Price.Open}',
                              high = '{price.Price.High}',
                              low = '{price.Price.High}',
                              close = '{price.Price.Close}',
                              volume = '{price.Volume}';";

            using var conn = DbConnections.CreatePsqlConnection();
            conn.Execute(sql);
        }

        /// <summary>
        /// Save security to database. If security exists in database it will be updated.
        /// </summary>
        /// <param name="security"></param>
        public static void SaveSecurity(Domain.Security security)
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
    }
}