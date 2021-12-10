using Crunch.Domain.Strategies;
using System;

namespace Crunch.Strategies.Crametorium
{
    class CrametoriumStrategy : IStrategy
    {
        public string Name { get; } = "Crametorium Strategy";

        public void RunAnalytics(DateOnly date)
        {
            Console.WriteLine(Name);
        }

        public void CreateDataSource()
        {
            Console.WriteLine("Crametorium data source");
        }
    }
}
