using System;

namespace Crunch.Core
{
    public record Symbol
    {
        public string Value { get; init; }

        public Symbol(string value)
        {
            if (value.Length > 4)
            {
                throw new ArgumentException($"Symbol {value} -> cannot be more than 4 characters");
            }
            if (value.Contains('-'))
            {
                throw new ArgumentException($"Symbol {value} -> cannot contain dash");
            }
            this.Value = value.ToUpper();
        }
    }
}