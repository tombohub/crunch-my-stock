using System;

namespace Crunch.Domain
{
    public class Symbol
    {
        public string Value { get; init; }

        public Symbol(string value)
        {
            if (value.Length > 4)
            {
                throw new ArgumentException("Symbol cannot be more than 4 characters");
            }
            this.Value = value.ToUpper();
        }
    }
}