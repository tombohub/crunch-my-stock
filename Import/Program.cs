namespace CrunchImport
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            switch (args[0])
            {
                case "prices":
                    await Service.ImportTodaysPrices();
                    break;

                case "securities":
                    Service.UpdateSecurities();
                    break;

                default:
                    Console.WriteLine("Valid commands are: 'prices', 'securities'");
                    break;
            }
        }
    }
}