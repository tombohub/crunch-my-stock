using CommandDotNet;
using Crunch.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            public void Prices(PeriodOptions periodOptions)
            {
                if (periodOptions.Today)
                {
                    var currentDateTime = DateTime.Now;
                    Console.WriteLine($"Current date and time is: {currentDateTime}");
                    var todayDate = DateOnly.FromDateTime(currentDateTime);
                    _app.ImportPrices(todayDate);
                }
                else if (periodOptions.Date != null)
                {
                    _app.ImportPrices(periodOptions.Date.Value);
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
        /// <param name="date"></param>
        /// <param name="securityType"></param>
        public void Plot([Option] DateOnly date, [Option] SecurityType securityType)
        {
            _app.Plot(date, securityType);
        }
    }

    public class PeriodOptions : IArgumentModel, IValidatableObject
    {
        [Option]
        public DateOnly? Date { get; set; }

        [Option]
        public bool Today { get; set; }

        [Option]
        public DateOnly? Start { get; set; }

        [Option]
        public DateOnly? End { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((Date != null) && Today)
            {
                yield return new ValidationResult("Cannot choose the date and today at the same time");
            }
            else if ((Date != null) && (Start != null || End != null))
            {
                yield return new ValidationResult("Choose either date or period (start, end)");
            }
            else if (Today && (Start != null || End != null))
            {
                yield return new ValidationResult("Choose either today or period (start, end)");
            }
        }
    }

}