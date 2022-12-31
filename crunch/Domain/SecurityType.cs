using System;
using System.Linq;
using Crunch.Extensions;

namespace Crunch.Domain
{
    /// <summary>
    /// Type of the given security (stock, etf...)
    /// </summary>
    public record SecurityType
    {
        public string Value { get; set; }
        public SecurityType(string value)
        {
            Validate(value);
            Value = value.ToLower().Capitalize();
        }

        private void Validate(string value)
        {
            if (!Constants.AllowedSecurityTypes.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Security type {value} is not allowed.", nameof(value));
            }
        }
    }
}