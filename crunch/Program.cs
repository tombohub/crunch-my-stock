using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Analysis;
using Crunch.UseCases;
using CommandLine;
using ScottPlot;
using Crunch.Strategies.Overnight;
using Crunch.DataSources.Fmp;
using Crunch.Domain;

namespace Crunch
{
    class Program
    {
        [Verb("import", HelpText = "Import prices and other data")]
        class ImportOptions
        {
            [Option("overnight", HelpText = "Import prices for weekly overnight strategy", SetName = "overnight")]
            public bool IsOvernight { get; set; }

            [Option('w', "week", HelpText = "Week number", SetName = "overnight")]
            public int WeekNum { get; set; }
        }

        [Verb("report", HelpText = "Calculate reports")]
        class ReportOptions
        {
            [Option('n', "name", HelpText = "Name of the report")]
            public string Name { get; set; }

            [Option('w', "week", HelpText = "Calendar week number", Required = true)]
            public int WeekNum { get; set; }
        }

        [Verb("plot", HelpText = "Plot reports")]
        class PlotOptions
        {
            [Option('n', "name", HelpText = "Name of the plot", Required = true)]
            public string Name { get; set; }

            [Option('w', "week", HelpText = "Calendar week number", Required = true)]
            public int WeekNum { get; set; }

            [Option('t', "type", HelpText = "Security type", Required =true)]
            public SecurityType SecurityType { get; set; }
        }

        [Verb("update", HelpText = "Update the list of securities in database")]
        class UpdateOptions { }
        

        private static void Main(string[] args)
        {

            Parser.Default.ParseArguments<UpdateOptions, ImportOptions, PlotOptions, ReportOptions>(args)
               .WithParsed<UpdateOptions>(options =>
               {
                   UseCase.UpdateSecurities();
               })
              .WithParsed<ImportOptions>(options =>
               {
                   UseCase.ImportPricesForOvernight(options.WeekNum);
               })
              .WithParsed<PlotOptions>(options =>
              {
                  var plottingUseCases = new PlottingUseCases(options.WeekNum);

                  if (options.Name == "winners") plottingUseCases.PlotWinnersLosersUseCase(options.SecurityType);
                  if (options.Name == "top10") plottingUseCases.PlotTop10UseCase(options.SecurityType);
                  if (options.Name == "bottom10") plottingUseCases.PlotBottom10UseCase(options.SecurityType);
                  if (options.Name == "spyroi") plottingUseCases.DrawSpyBenchmarkRoiUseCase();
                  if (options.Name == "spyovernightroi") plottingUseCases.DrawSpyOvernightRoiUseCase();
                  if (options.Name == "averageroi") plottingUseCases.DrawAverageOvernightRoiUseCase(options.SecurityType);
                  if (options.Name == "benchmarkroi") plottingUseCases.DrawAverageBenchmarkRoiUseCase();
                  if (options.Name == "all") plottingUseCases.PlotOvernightUseCase(options.SecurityType);

              });
        }

    }
}


