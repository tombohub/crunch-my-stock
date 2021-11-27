using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain;
using Crunch.UseCases;


namespace Crunch.CLIControllers
{
    /// <summary>
    /// Responsible for mapping command line arguments to run Download Prices use case.
    /// </summary>
    internal class DownloadPriceController : ICliController
    {
        private readonly TimeRange _timeRange;
        private readonly PriceInterval _interval;

        /// <summary>
        /// Initialize Download Prices controller
        /// </summary>
        /// <param name="start">Prices data start date</param>
        /// <param name="end">Prices data end date</param>
        /// <param name="interval">Time interval between each price</param>
        public DownloadPriceController(string start, string end, string interval)
        {
            try
            {
                _timeRange = new TimeRange(DateTime.Parse(start), DateTime.Parse(end));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Did you write start and end date in yyyy-mm-dd format?");
            }

            _interval = interval switch
            {
                "30m" => PriceInterval.ThirtyMinutes,
                "1d" => PriceInterval.OneDay,
                _ => throw new ArgumentException($"Interval {interval} doesn't exist")
            };
        }

        /// <summary>
        /// Runs prices download use case
        /// </summary>
        public void RunUseCase()
        {
            var useCase = new ImportPricesUseCase(_timeRange, _interval);
            useCase.Execute();
        }

    }
}
