using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Domain.Strategies
{
    /// <summary>
    /// Responsible for analyzing data and creating new metrics and stats
    /// </summary>
    internal interface IAnalyzer
    {
        public void Run();
    }
}
