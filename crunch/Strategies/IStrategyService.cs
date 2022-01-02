using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Strategies
{
    /// <summary>
    /// Interface for facade which provides strategy services like plotting,
    /// analyzing etc on the scope of the whole strategy.
    /// </summary>
    internal interface IStrategyService
    {
        public void Plot();
    }
}
