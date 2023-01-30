using System.Collections.Generic;
using System.Linq;

namespace Crunch.Core
{
    internal class DomainService
    {
        public List<SecurityPriceOvernight> TransformToOvernightPrices(List<SecurityPrice> previousDayPrices, List<SecurityPrice> todayPrices)
        {
            var prices = previousDayPrices
                .Join(
                todayPrices,
                prev => prev.Symbol.Value,
                today => today.Symbol.Value,
                (prev, today) => new SecurityPriceOvernight(prev, today)
                     )
                .ToList();

            return prices;
        }
    }
}