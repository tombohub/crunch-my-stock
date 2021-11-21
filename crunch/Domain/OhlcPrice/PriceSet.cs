using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crunch.Domain.OhlcPrice
{
    record PriceSet(
        string Symbol,
        PriceInterval interval,
        List<Price> Prices
        );

    record Price(
        DateTime Timestamp,
        double Open,
        double High,
        double Low,
        double Close,
        ulong Volume
    );
}
