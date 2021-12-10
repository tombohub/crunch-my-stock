using System;

namespace Crunch.Strategies.Overnight.CliControllers
{
    internal class RunAnalyticsCommandController
    {
        private DateOnly _date;
        public RunAnalyticsCommandController(string date)
        {
            try
            {
                _date = DateOnly.Parse(date);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Did you write start and end date in yyyy-mm-dd format?");
            }
        }

        public void RunCommand()
        {
            Console.WriteLine("Running command line controller for overnight run analytics command");
            var strategy = new OvernightStrategy();
            strategy.RunAnalytics(_date);
        }
    }
}
