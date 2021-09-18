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
        private static void Main(string[] args)
        {
            if (args[0] == "import:prices")
            {
                UseCase.ImportPricesForOvernight(35);

            }
            else if (args[0] == "plot")
            {
                UseCase.SynchronizeSecurities();
            }

        }

    }
}


