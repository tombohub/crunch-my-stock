using CommandDotNet;

namespace CrunchImport
{
    internal class CommandLineParser
    {
        public void Prices([Option] DateOnly? date, [Option] bool today)
        {
            if (today)
            {
                var currentDateTime = DateTime.Now;
                Console.WriteLine($"Current date and time is: {currentDateTime}");
                var todayDate = DateOnly.FromDateTime(currentDateTime);
                Service.ImportPrices(todayDate);
            }
        }

        public void Securities()
        {
            Service.UpdateSecurities();
        }
    }
}