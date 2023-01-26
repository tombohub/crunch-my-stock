using System;
using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.Strategies.Overnight;

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
        /// Run strategy analytics command.
        /// Selects target security already downloaded prices and saves into the database table.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="date"></param>
        public void Run(
        [Option]
        StrategyName strategy,

        [Option]
        DateOnly date)
        {
            Console.WriteLine($"This is command line parser for the command 'run' {strategy}");
            Console.WriteLine($"Date is {date}");

            // get previous trading day from chosen date for overnight strategy it is
            var calendarDay = new CalendarDay(date);
            DateOnly prevTradingDay = calendarDay.PreviousTradingDay;

            var overnightDb = new OvernightDatabase();
            overnightDb.SavePrices(date, prevTradingDay);
        }

        /// <summary>
        /// Plot strategy reports all in one image
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="date"></param>
        public void Plot([Option] StrategyName strategy, [Option] DateOnly date)
        {
            // get data from database for each report
            // create individual plot
            // get coordinates for each plot to add in multiplot
            // create multiplot using individual plots and coordinates
            // save as image file
            IStrategyService strategyService = StrategyServiceFactory.CreateService(strategy);
            strategyService.CreateStrategyMultiplot(date);
        }
    }
}