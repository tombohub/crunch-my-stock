using System.Collections.Generic;

namespace Crunch.Core
{
    /// <summary>
    /// Represents daily prices across all securities for a day, in regular trading time
    /// 9:30 - 16:00
    /// </summary>
    internal record DailyPricesRegular
    {
        public required TradingDay TradingDay { get; init; }
        public required List<SecurityPrice> SecurityPrices { get; init; }
    }
}