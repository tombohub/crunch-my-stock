using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Strategies.Overnight;
using Crunch.Database;
using Dapper;
using Crunch.Domain;
using Crunch.Domain.Multiplots;
using Crunch.Strategies.Overnight.AreaContents;

namespace Crunch.Strategies.Overnight
{
    internal class AreaContentsRepository
    {
        /// <summary>
        /// Date of the reports
        /// </summary>
        private DateOnly _date;

        /// <summary>
        /// Databse connection object
        /// </summary>
        private Npgsql.NpgsqlConnection _connection;

        /// <summary>
        /// Initialize reports repository for the reports on the given date
        /// </summary>
        /// <param name="date"></param>
        public AreaContentsRepository(DateOnly date)
        {
            _date = date;
            _connection = DbConnections.CreatePsqlConnection();
        }

        /// <summary>
        /// Gets Average ROI from the database for the Overnight strategy on the given date
        /// </summary>
        /// <param name="date">Date for the Average ROI</param>
        /// <returns>Single metrics: average ROI in % for the strategy on the given date</returns>
        [Area(Name = "AvgRoi", Strategy = "Overnight")]
        public AvgRoiReport CreateAvgRoiStocksReport()
        {
            string sql = "SELECT average_roi FROM overnight.average_roi WHERE date = @Date";
            var parameters = new { Date = _date };

            decimal avgRoi = _connection.ExecuteScalar<decimal>(sql, parameters);
            return new AvgRoiReport(avgRoi);
        }

        /// <summary>
        /// Gets SPY ROI from the database for the Overnight strategy on the given date
        /// </summary>
        /// <param name="date">Date for the SPY ROI</param>
        /// <returns>Single metrics: SPY ROI in % for the strategy on the given date</returns>
        [Area(Name = "SpyRoi", Strategy = "Overnight")]
        public SpyRoiReport CreateSpyRoiReport()
        {
            string sql = "SELECT spy_roi FROM overnight.spy_roi WHERE date = @Date";
            var parameters = new { Date = _date };

            decimal spyRoi = _connection.ExecuteScalar<decimal>(sql, parameters);
            return new SpyRoiReport(spyRoi);
        }

        /// <summary>
        /// Gets report for number of winning and losing securities
        /// </summary>
        /// <returns>Winners and Losers Report object</returns>
        [Area(Name = "WinnersLosers", Strategy = "Overnight")]
        public WinnersLosersReport CreateWinnersLosersStocksReport()
        {
            string sql = @"SELECT winners_count, losers_count FROM overnight.winners_losers_count WHERE date = @Date";
            var parameters = new { Date = _date };
            var reportData = _connection.QuerySingle<WinnersLosersCount>(sql, parameters);
            return new WinnersLosersReport(reportData);
        }

        /// <summary>
        /// Create Bottom 10 report instance
        /// </summary>
        /// <returns>Bottom 10 report object instance</returns>
        [Area(Name = "Bottom10", Strategy = "Overnight")]
        public Bottom10Report CreateBottom10Report()
        {
            string sql = @"SELECT symbol, change_pct FROM overnight.bottom10_stocks WHERE date = @Date";
            var parameters = new {Date = _date };
            var reportData = _connection.Query<SecurityPerformance>(sql, parameters).ToList();
            return new Bottom10Report(reportData);
        }

        /// <summary>
        /// Create Top 10 report instance
        /// </summary>
        /// <returns>Bottom 10 report object instance</returns>
        [Area(Name = "Top10", Strategy = "Overnight")]
        public Top10Report CreateTop10Report()
        {
            string sql = @"SELECT symbol, change_pct FROM overnight.top10_stocks WHERE date = @Date";
            var parameters = new { Date = _date };
            var reportData = _connection.Query<SecurityPerformance>(sql, parameters).ToList();
            return new Top10Report(reportData);
        }

        [Area(Name = "WinnersLosersByPrice", Strategy = "Overnight")]
        public WinnersLosersByPriceReport CreateWinnersLosersByPriceReport()
        {
            string sql = @"SELECT winners_count, losers_count, price_upper_bound FROM overnight.winners_losers_count_by_price WHERE date = @Date";
            var parameters = new {Date = _date};
            var reportData = _connection.Query<WinnersLosersCountByPrice>(sql, parameters).ToList();
            return new WinnersLosersByPriceReport(reportData);
        }

        [Area(Name = "Title")]
        public Title CreateTitle()
        {
            return new Title(_date);
        }
    }
}
