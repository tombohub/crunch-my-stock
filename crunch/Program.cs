using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.UseCases;
using CommandLine;
using CommandLine.Text;
using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.Strategies.Overnight.CliControllers;
using Crunch.CLIControllers;

namespace Crunch
{
    class Program
    {
        //[Verb("test")]
        //class TestOptions
        //{
            
        //}

        //[Verb("download", HelpText ="Download all securities prices data into the database")]
        //class DownloadOptions
        //{
        //    [Option("start", Required =true, HelpText ="Prices starting date, format: yyyy-mm-dd")]
        //    public string Start { get; set; }

        //    [Option("end", Required =true,HelpText ="Prices ending date, inclusive, format: yyyy-mm-dd")]
        //    public string End { get; set; }

        //    [Option("interval", Required = true, HelpText = "Prices interval, choices: [30m, 1d]")]
        //    public string Interval { get; set; }
        //}

        ///// <summary>
        ///// Options to run analytics for the chosen strategy
        ///// </summary>
        //[Verb("analyze", HelpText = "run analytics for the chosen strategy")]
        //class AnalyzeOptions
        //{
        //    [Option("strategy", HelpText = "Name of the strategy", Required = true)]
        //    public Strategy StrategyName { get; set; }


        //    [Option("date", HelpText = "Date to analyze", Required = true)]
        //    public string Date { get; set; }
        //}

        [Command(Description ="Perform calculations")]
        public class Calculator
        {
            [Command(Description = "Adds two numbers")]
            public void Add(int value1, int value2)
            {
                Console.WriteLine($"Answer:  {value1 + value2}");
            }

            [Command(Description = "Subtracts two numbers")]
            public void Subtract(int value1, int value2)
            {
                Console.WriteLine($"Answer:  {value1 - value2}");
            }
        }


        private static void Main(string[] args)
        {
            new AppRunner<Calculator>().Run(args);
            //var parser = new Parser(with =>
            //{
            //    with.CaseInsensitiveEnumValues = true;
            //});

            //var result = parser.ParseArguments<TestOptions, DownloadOptions, AnalyzeOptions>(args);

            //result.WithParsed<TestOptions>(o =>
            //{
            //    var useCase = new RunOvernightAnalyticsUseCase();
            //    useCase.Execute();
            //})
            //.WithParsed<DownloadOptions>(o =>
            //{
            //    var controller = new DownloadPriceController(o.Start, o.End, o.Interval);
            //    controller.RunUseCase();
            //})
            //.WithParsed<AnalyzeOptions>(o =>
            //{
            //    //TODO: implement
            //    Console.WriteLine($"This is command line parser for the command 'analyze' {o.StrategyName}");
            //    Console.WriteLine($"Date is {o.Date}");
            //    Strategy strategy;

            //    var controller = CliControllersFactory.CreateCliController(o.StrategyName);
            //    controller.RunAnalytics(o.Date);

            //});
        }
    }
}


