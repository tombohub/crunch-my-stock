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
            [Option('w', "week", HelpText = "Calendar week number", Required = true)]
            public int WeekNum { get; set; }
        }

        [Verb("plot", HelpText = "Plot reports")]
        class PlotOptions
        {
            [Option('w', "week", HelpText = "Calendar week number", Required = true)]
            public int WeekNum { get; set; }
        }

        [Verb("update", HelpText = "Update the list of securities in database")]
        class UpdateOptions { }


        private static void Main(string[] args)
        {

            Parser.Default.ParseArguments<UpdateOptions, ImportOptions, PlotOptions>(args)
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
                  UseCase.PlotWinnersLosers(options.WeekNum);
              });
        }

    }
}


