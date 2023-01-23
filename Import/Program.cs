namespace CrunchImport
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            switch (args[0])
            {
                case "prices":
                    Service.ImportTodaysPrices();
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