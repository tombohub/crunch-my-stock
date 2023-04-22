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
            /// <param name="importPricesOptions"></param>
            public void Prices(ImportPricesOptions importPricesOptions)
            {
                if (importPricesOptions.Today)
                {
                    var currentDateTime = DateTime.Now;
                    Console.WriteLine($"Current date and time is: {currentDateTime}");
                    var todayDate = DateOnly.FromDateTime(currentDateTime);

                    _app.ImportPrices(todayDate);
                }

                else if (importPricesOptions.Date != null)
                {
                    _app.ImportPrices(importPricesOptions.Date.Value);
                }

                else if (importPricesOptions.PeriodOptions.Start.HasValue && importPricesOptions.PeriodOptions.End.HasValue)
                {
                    var start = importPricesOptions.PeriodOptions.Start;
                    var end = importPricesOptions.PeriodOptions.End;

                    Console.WriteLine($"start: {start} end: {end}");
                }
                else if (importPricesOptions.PeriodOptions.Start.HasValue && importPricesOptions.PeriodOptions.End == null)
                {
                    var start = importPricesOptions.PeriodOptions.Start;
                    var end = importPricesOptions.PeriodOptions.End;

                    Console.WriteLine($"start: {start}, end: {end}");
                }
            }

            /// <summary>
            /// Updates all securities in database
            /// </summary>
            public void Securities()
            {
                _app.UpdateSecurities();
            }
        }

        public void SaveOvernightPrices(DateOnly date)
        {
            _app.SaveOvernightPrices(date);
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

    public class ImportPricesOptions : IArgumentModel, IValidatableObject
    {
        /// <summary>
        /// Specific trading day 
        /// </summary>
        [Option]
        public DateOnly? Date { get; set; } = null;

        /// <summary>
        /// Today's date
        /// </summary>
        [Option]
        public bool Today { get; set; } = false;

        /// <summary>
        /// Date period with start date and end date
        /// </summary>
        [Option]
        public PeriodOptions PeriodOptions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date.HasValue && (Today || PeriodOptions.Start.HasValue || PeriodOptions.End.HasValue))
            {
                yield return new ValidationResult("Choose only date, today or period (start, end)1");
                yield break;
            }
            else if (Today && (Date.HasValue || PeriodOptions.Start.HasValue || PeriodOptions.End.HasValue))
            {
                yield return new ValidationResult("Choose only date, today or period (start, end)2");
                yield break;
            }
            else if (PeriodOptions.Start.HasValue && (Date.HasValue || Today))
            {
                yield return new ValidationResult("Choose only date, today or period (start, end)3");
                yield break;
            }
            else if (PeriodOptions.End.HasValue && PeriodOptions.Start == null)
            {
                yield return new ValidationResult("Have to use --start together with --end");
                yield break;
            }
            else if (PeriodOptions.Start > PeriodOptions.End)
            {
                yield return new ValidationResult("--start has to be earlier than --end");
                yield break;
            }

        }

    }
    public class PeriodOptions : IArgumentModel
    {
        [Option]
        public DateOnly? Start { get; set; } = null;

        [Option]
        public DateOnly? End { get; set; } = null;
    }

}