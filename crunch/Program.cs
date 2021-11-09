using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.UseCases;
using CommandLine;
using Crunch.Domain;

namespace Crunch
{
    class Program
    {
        class Options
        {
            [Option("strategy", Required =true, HelpText ="Name of the strategy")]
            public string StrategyName { get; set; }
        }
        

        private static void Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(o =>
               {
                   var useCase = new PrintStrategyNameUseCase(o.StrategyName);
                   useCase.Execute();
               });
             
        }

    }
}


