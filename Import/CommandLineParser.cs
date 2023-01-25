namespace CrunchImport
{
    internal class CommandLineParser
    {
        public void Prices()
        {
            var currentDateTime = DateTime.Now;
            Console.WriteLine($"Current date and time is: {currentDateTime}");
            DateOnly date = DateOnly.FromDateTime(currentDateTime);

            Service.ImportPrices(date);
        }

        public void Securities()
        {
            Service.UpdateSecurities();
        }
    }
}