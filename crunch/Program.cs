using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.DataSource;
using Crunch.Core;
using Crunch.Database;
using Microsoft.Data.Analysis;
using Crunch.Database.Models;
using Crunch.UseCases;
using CommandLine;
using ScottPlot;
using Crunch.Strategies.Overnight;

namespace Crunch
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args[0] == "import:prices")
            {
                UseCase.ImportPrices();

            }
            else if (args[0] == "plot")
            {
                var p = new PriceDownloadOptions(36);
                Console.Write(p.Start);
            }

        }

    }
}


