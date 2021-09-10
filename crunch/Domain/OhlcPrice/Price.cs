using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crunch.Domain.OhlcPrice
{
    record Price(
        string symbol,
        DateTime timestamp,
        double open,
        double high,
        double low,
        double close,
        ulong volume,
        PriceInterval interval);
}
