using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.UseCases;
using CommandLine;
using Crunch.Domain;
using Crunch.CLIControllers;

namespace Crunch
{
    class Program
    {
        [Verb("test")]
        class TestOptions
        {
            
        }

        [Verb("download", HelpText ="Download all securities prices data into the database")]
        class DownloadOptions
        {
            [Option("start", Required =true, HelpText ="Prices starting date, format: yyyy-mm-dd")]
            public string Start { get; set; }

            [Option("end", Required =true,HelpText ="Prices ending date, inclusive, format: yyyy-mm-dd")]
            public string End { get; set; }

            [Option("interval", Required = true, HelpText = "Prices interval, choices: [30m, 1d]")]
            public string Interval { get; set; }
        }

        

        private static void Main(string[] args)
        {

            Parser.Default.ParseArguments<TestOptions, DownloadOptions>(args)
               .WithParsed<TestOptions>(o =>
               {
                   var useCase = new RunOvernightAnalyticsUseCase();
                   useCase.Execute();
               })
               .WithParsed<DownloadOptions>(o =>
               {
                   var controller = new DownloadPriceController(o.Start, o.End, o.Interval);
                   controller.RunUseCase();
               });
        }
    }
}


