﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Database;
using Dapper;

namespace Crunch.Strategies.Overnight
{
    internal class OvernightDatabase
    {
        private Npgsql.NpgsqlConnection _connection;

        public OvernightDatabase()
        {
            _connection = DbConnections.CreatePsqlConnection();
        }


        /// <summary>
        /// Save prices needed for overnight strategy.
        /// Calls stored procedure which selects prices from general prices table
        /// and inserts into Overnight strategy prices table
        /// </summary>
        /// <param name="strategyDate"></param>
        /// <param name="prevTradingDay"></param>
        public void SavePrices(DateOnly strategyDate, DateOnly prevTradingDay)
        {
            if (CheckPricesExist(strategyDate, prevTradingDay))
            {
                string sql = $"CALL overnight.insert_overnight_prices(@StrategyDate, @PrevTradingDay);";
                var parameters = new { StrategyDate = strategyDate, PrevTradingDay = prevTradingDay };
                _connection.Execute(sql, parameters);
            }
            else Console.WriteLine($"There's no prices data for the {strategyDate}. Download prices first.");
        }


        /// <summary>
        /// Check if prices for the selected days are in database
        /// </summary>
        /// <param name="strategyDate"></param>
        /// <param name="prevTradingDay"></param>
        /// <returns></returns>
        private bool CheckPricesExist(DateOnly strategyDate, DateOnly prevTradingDay)
        {
            string strategyDatePricesSql = "SELECT count(*) FROM overnight.overnight_daily_stats WHERE date = @StrategyDate;";
            string prevTradingDayPricesSql = "SELECT count(*) FROM overnight.overnight_daily_stats WHERE date = @PrevTradingDay;";
            var parameters = new { StrategyDate = strategyDate, PrevTradingDay = prevTradingDay };
            int strategyDateRowCount = _connection.Execute(strategyDatePricesSql, parameters);
            int prevTradingDayRowCount = _connection.Execute(prevTradingDayPricesSql, parameters);

            return (strategyDateRowCount > 0) && (prevTradingDayRowCount > 0);
        }

    }
}