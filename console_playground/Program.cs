using Crunch.Core;
using Crunch.Database;

namespace console_playground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseMethods();
            var price = new SecurityPrice
            {
                Symbol = new Symbol("AAPL"),
                OHLC = new OHLC(2, 4, 1, 2),
                SecurityType = SecurityType.Stock,
                TradingDay = new TradingDay(new DateOnly(2023, 3, 8)),
                Volume = 32222
            };
            db.SaveDailyPrice(price);
        }
    }


}