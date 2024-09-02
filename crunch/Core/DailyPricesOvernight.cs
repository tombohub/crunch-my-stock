using System.Collections.Generic;

namespace Crunch.Core
{
    /// <summary>
    /// Prices across all securities for the day during overnight time,
    /// 16:00 - 09:30 next day
    /// </summary>
    public record DailyPricesOvernight
    {
        public required TradingDay TradingDay { get; init; }
        public required TradingDay PrevTradingDay { get; init; }
        public required List<SecurityPriceOvernight> Prices { get; init; }
    }
}