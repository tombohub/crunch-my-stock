using System;
using System.Collections.Generic;

namespace Crunch.Domain
{
    /// <summary>
    /// Represents set of daily prices related to single symbol
    /// </summary>
    /// <param name="Symbol"></param>
    /// <param name="Interval"></param>
    /// <param name="Prices"></param>
    record DailyPriceSet(
        string Symbol,
        PriceInterval Interval,
        List<PriceDaily> Prices
        );

    record PriceDaily(
        DateOnly Timestamp,
        decimal Open,
        decimal High,
        decimal Low,
        decimal Close,
        long Volume
    );
}
