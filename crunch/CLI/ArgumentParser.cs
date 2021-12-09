using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.UseCases;

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
        public void Download(DateOnly start, DateOnly end, PriceInterval interval)
        {
            Console.WriteLine(start);
            Console.WriteLine(end);
            Console.WriteLine(interval);
            var dateRange = new Period(start, end);
            var useCase = new DownloadPricesUseCase(dateRange, interval);
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
        }
    }
}
