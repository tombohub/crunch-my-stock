using System;
using System.Collections.Generic;

namespace Crunch.Domain
{
    /// <summary>
    /// Represents set of intraday prices related to single symbol
    /// </summary>
    /// <param name="Symbol"></param>
    /// <param name="Interval"></param>
    /// <param name="Prices"></param>
    record IntradayPriceSet(
        string Symbol,
        PriceInterval Interval,
        List<PriceIntraday> Prices
        );

    record PriceIntraday(
        DateTime Timestamp,
        double Open,
        double High,
        double Low,
        double Close,
        int Volume
    );
}
