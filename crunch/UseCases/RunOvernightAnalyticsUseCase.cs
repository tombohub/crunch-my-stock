using Crunch.Database;
using System;

namespace Crunch.UseCases
{
    internal class RunOvernightAnalyticsUseCase : IUseCase
    {
        /// <summary>
        /// Repository object for getting prices data
        /// </summary>
        private DailyPriceSetRepository _priceSetRepo = new DailyPriceSetRepository();

        public void Execute()
        {
            Console.WriteLine("Executing Run overnight analytics use case");
            // get prices for each symbol from database for the previous trading day.
            // To do that we need to get previous trading day.

            ////var today = new CalendarDay(new DateOnly(2021, 11, 19));
            ////Console.WriteLine(today.IsTradingDay);
            ////Console.WriteLine($"Previous trading day - {today.PreviousTradingDay}");
            ////DailyPriceSet priceSet = _priceSetRepo.Load("SPY", PriceInterval.OneDay, today.PreviousTradingDay, today.Date);
            ////Console.WriteLine(priceSet.Symbol);
            ////Console.WriteLine(priceSet.Prices[0].Open);
            ////Console.WriteLine($"date of price - {priceSet.Prices[0].Timestamp}");

            // calculate metrices
            // save those metrics to database
        }
    }
}
