using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain
{
    public class Security
    {
        public string Symbol { get; init; }
        public SecurityType Type { get; init; }

        public Security(string symbol, SecurityType type)
        {
            Symbol = symbol;
            Type = type;
        }
    }
}
