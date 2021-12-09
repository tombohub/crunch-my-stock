﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database.Models;
using Crunch.Domain;
using Microsoft.EntityFrameworkCore;

namespace Crunch.Database
{
    /// <summary>
    /// General methods for performing various tasks
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// Truncate the prices table. 
        /// </summary>
        /// <param name="interval">Price interval of the price data</param>
        /// <remarks>Reasong for truncating is because many times inserted prices
        /// are with same symbol and date, which is unique key. In that case MySql creates error</remarks>
        public static void TruncatePricesTable(PriceInterval interval)
        {
            string tableName = interval switch
            {
                PriceInterval.OneDay => "prices_daily",
                PriceInterval.ThirtyMinutes => "prices_intraday",
                _ => throw new ArgumentException("Interval not supported", nameof(interval))
            };

            try
            {
                var db = new stock_analyticsContext();
                db.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
                db.Dispose();
                Console.WriteLine($"{tableName} table truncated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

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
    }
}