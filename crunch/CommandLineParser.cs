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
        private readonly ApplicationService _app = new ApplicationService();

        [Subcommand]
        internal class Import
        {
            private readonly ApplicationService _app = new ApplicationService();

            /// <summary>
            /// Import prices into the database command for all securities
            /// </summary>
            /// <param name="date">TradingDay for the prices</param>
            /// <param name="today">Import today's prices</param>
            public void Prices([Option] DateOnly? date, [Option] bool today)
            {
                if (today)
                {
                    var currentDateTime = DateTime.Now;
                    Console.WriteLine($"Current date and time is: {currentDateTime}");
                    var todayDate = DateOnly.FromDateTime(currentDateTime);
                    _app.ImportPrices(todayDate);
                }
                else if (date != null)
                {
                    _app.ImportPrices(date.Value);
                }
            }

            public void OvernightPrices(DateOnly date)
            {
                _app.ImportOvernightPrices(date);
            }

            /// <summary>
            /// Updates all securities in database
            /// </summary>
            public void Securities()
            {
                _app.UpdateSecurities();
            }
        }

        public void Analyze([Option] DateOnly date)
        {
            _app.Analyze(date);
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