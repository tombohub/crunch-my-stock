using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.UseCases;
using Dapper;
using System;
using System.Data;
using System.Collections.Generic;
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
        public void Plot([Option] Strategy strategy)
        {
            // get data from database for each report
            // create individual plot
            // get coordinates for each plot to add in multiplot
            // create multiplot using individual plots and coordinates
            // save as image file
            IStrategyFacade strategyFacade = StrategyFacadeFactory.CreateFacade(strategy);
            strategyFacade.Plot();
            
        }
    }
}
