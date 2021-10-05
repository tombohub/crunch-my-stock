using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies.Overnight
{
    public static class StrategyExtensions
    {
        public static string GetString(this Strategy strategy)
        {
            switch (strategy)
            {
                case Strategy.Overnight:
                    return "overnight";
                case Strategy.Benchmark:
                    return "benchmark";
                default:
                    throw new MethodAccessException("Method can be used only on Strategy enum");
            }
        }
    }
}
