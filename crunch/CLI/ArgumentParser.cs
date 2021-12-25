using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.UseCases;
using Dapper;
using System;
using System.Data;
using System.Collections.Generic;

namespace Crunch.CLI
{
    /// <summary>
    /// Parser for command line arguments.
    /// Each method is single command in CLI.
    /// Acts as a controller too - calls specific use cases.
    /// </summary>
    internal class ArgumentParser
    {
        /// <summary>
        /// Download prices CLI command
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="interval"></param>
        public void Download([Option] DateOnly start, [Option] DateOnly end, [Option] PriceInterval interval)
        {
            Console.WriteLine(start);
            Console.WriteLine(end);
            Console.WriteLine(interval);
            var period = new Period(start, end);
            var useCase = new DownloadPricesUseCase(period, interval);
            useCase.Execute();
        }

        /// <summary>
        /// Run strategy analytics command
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="date"></param>
        public void Run(
        [Option]
        Strategy strategy,

        [Option]
        DateOnly date)
        {
            //TODO: implement
            Console.WriteLine($"This is command line parser for the command 'run' {strategy}");
            Console.WriteLine($"Date is {date}");

            // get previous trading day from chosen date for overnight strategy it is
            var calendarDay = new CalendarDay(date);
            DateOnly prevTradingDay = calendarDay.PreviousTradingDay;

            // take prices from previous trading day
            var conn = Database.DbConnections.CreatePsqlConnection();
            conn.Open();
            string sql = $"CALL overnight.insert_overnight_prices('{date}','{prevTradingDay}');";
            conn.Execute(sql);
            conn.Close();
        }

        record OvernightStrategyPriceDTO
        {
            public DateOnly StrategyDate { get; set; }
            public string Symbol { get; set; }
            public SecurityType SecurityType { get; set; }
            public double StartPrice { get; set; }
            public double EndPrice { get; set; }
        }

       
    }
}
