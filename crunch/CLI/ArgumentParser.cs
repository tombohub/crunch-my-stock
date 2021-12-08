using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.UseCases;

namespace Crunch.CLI
{
    /// <summary>
    /// Parser for command line arguments.
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
            var dateRange = new DateRange(start, end);
            var useCase = new DownloadPricesUseCase(dateRange, interval);
            useCase.Execute();
        }
    }
}
