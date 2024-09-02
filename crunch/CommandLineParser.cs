using CommandDotNet;
using Crunch.Core;
using System;

namespace Crunch
{
    /// <summary>
    /// Parser for command line arguments.
    /// Each method is single command in CLI.
    /// Acts as a controller too - calls specific use cases.
    /// </summary>
    internal class CommandLineParser
    {
        /// <summary>
        /// Service
        /// </summary>
        private readonly ApplicationService _app = new ApplicationService();

        [Subcommand]
        internal class Import
        {
            private readonly ApplicationService _app = new ApplicationService();



            /// <summary>
            /// Updates all securities in database
            /// </summary>
            public void Securities()
            {
                _app.UpdateSecurities();
            }
        }

        public void SaveOvernightPrices([Option] DateOnly? date)
        {
            if (date.HasValue)
            {
                _app.SaveOvernightPrices(date.Value);
            }
            else
            {
                _app.SaveOvernightPrices();
            }
        }

        /// <summary>
        /// Plot strategy reports all in one image
        /// </summary>
        /// <param name="date"></param>
        /// <param name="securityType"></param>
        public void Plot([Option] DateOnly date, [Option] SecurityType securityType)
        {
            _app.Plot(date, securityType);
        }
    }



}