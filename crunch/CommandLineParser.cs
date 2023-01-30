using System;
using CommandDotNet;
using Crunch.Core;
using Crunch.Strategies;

namespace Crunch
{
    /// <summary>
    /// Parser for command line arguments.
    /// Each method is single command in CLI.
    /// Acts as a controller too - calls specific use cases.
    /// </summary>
    internal class CommandLineParser
    {
        [Subcommand]
        internal class Import
        {
            /// <summary>
            /// Import prices into the database command for all securities
            /// </summary>
            /// <param name="date">Date for the prices</param>
            /// <param name="today">Import today's prices</param>
            public void Prices([Option] DateOnly? date, [Option] bool today)
            {
                if (today)
                {
                    var currentDateTime = DateTime.Now;
                    Console.WriteLine($"Current date and time is: {currentDateTime}");
                    var todayDate = DateOnly.FromDateTime(currentDateTime);
                    ApplicationService.ImportPrices(todayDate);
                }
            }

            /// <summary>
            /// Updates all securities in database
            /// </summary>
            public void Securities()
            {
                ApplicationService.UpdateSecurities();
            }
        }

        public void Analyze([Option] DateOnly date)
        {
            ApplicationService.Analyze(date);
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