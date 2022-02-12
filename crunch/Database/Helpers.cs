using Crunch.Database.Models;
using Crunch.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Crunch.Strategies;
using Dapper;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    internal class Helpers
    {

        /// <summary>
        /// Get list of all symbols from database
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSecuritySymbols()
        {
            var db = new stock_analyticsContext();
            var symbols = db.Securities.Select(s => s.Symbol).ToList();
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
    }
}
