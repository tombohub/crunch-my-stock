using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.UseCases;
using Dapper;
using System;

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
        public void Analyze(
        [Option]
        Strategy strategy,

        [Option]
        DateOnly date)
        {
            //TODO: implement
            Console.WriteLine($"This is command line parser for the command 'analyze' {strategy}");
            Console.WriteLine($"Date is {date}");

            // get previous trading day from chosen date
            var calendarDay = new CalendarDay(date);
            DateOnly prevTradingDay = calendarDay.PreviousTradingDay;

            // take prices from previous trading day
            var conn = Database.MySqlDatabase.GetConnection();
            var res = conn.Query<Res>($"select `symbol`,`close` from prices_daily pd where `timestamp` = '{prevTradingDay}'");
            foreach (var r in res)
            {
                Console.WriteLine(r.Symbol);
            }

            // compare the previous close price with current date open price
            // calculate stats - percent change
            // save stats to database
        }

        class Res
        {
            public string Symbol { get; set; }
            public double Open { get; set; }
        }
    }
}
