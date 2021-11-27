using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crunch.Domain.OhlcPrice
{
    /// <summary>
    /// Represents set of Price related to single symbol
    /// </summary>
    /// <param name="Symbol"></param>
    /// <param name="Interval"></param>
    /// <param name="Prices"></param>
    record PriceSet(
        string Symbol,
        PriceInterval Interval,
        List<Price> Prices
        );

    record Price(
        DateTime Timestamp,
        double Open,
        double High,
        double Low,
        double Close,
        int Volume
    );
}
