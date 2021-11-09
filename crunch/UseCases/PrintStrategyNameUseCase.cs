using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crunch.Domain.Strategies;
using Crunch.Strategies.Overnight;
using Crunch.Strategies.Crametorium;

namespace Crunch.UseCases
{
    class PrintStrategyNameUseCase
    {
        private IStrategy Strategy { get; init; } 
        public PrintStrategyNameUseCase(string strategyOption)
        {
            Strategy = strategyOption switch
            {
                "overnight" => new OvernightStrategy(),
                "crametorium" => new CrametoriumStrategy(),
                _ => throw new NotImplementedException($"{strategyOption} strategy is not yet implemented")
            };
        }

        public void Execute()
        {
            Strategy.PrintStrategyName();
        }
    }
}
