using System;
using System.Linq;

namespace Crunch.Domain
{
    public record Exchange
    {
        public string Value { get; init; }
        public Exchange(string value)
        {
            Validate(value);
            Value = value.ToUpper();
        }

        private void Validate(string value)
        {
            if (!Constants.AllowedExchanges.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Exchange {value} is not allowed");
            }
        }
    }
}