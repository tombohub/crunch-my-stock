using System;
using System.Collections.Generic;
using System.Linq;

namespace Crunch.Core
{
    /// <summary>
    /// Represents OHLC price
    /// </summary>
    public record OHLC
    {
        public decimal Open { get; init; }
        public decimal High { get; init; }
        public decimal Low { get; init; }
        public decimal Close { get; init; }

        public OHLC(decimal open, decimal high, decimal low, decimal close)
        {
            Validate(open, high, low, close);
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// Construct when only open and close is needed
        /// </summary>
        /// <param name="open"></param>
        /// <param name="close"></param>
        public OHLC(decimal open, decimal close)
        {
            Open = open;
            Close = close;
        }

        private void Validate(decimal open, decimal high, decimal low, decimal close)
        {
            List<decimal> list = new List<decimal> { open, high, low, close };
            if (list.Max(x => x) != high)
            {
                throw new ArgumentException($"High price of {high} is not the highest in OHLC");
            }

            if (list.Min(x => x) != low)
            {
                throw new ArgumentException($"Low price of {low} is not the lowest in OHLC");
            }
        }
    }
}