using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.UseCases;
using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.Strategies.Overnight.CliControllers;
using Crunch.CLI.Controllers;
using Crunch.CLI;

namespace Crunch
{
    class Program
    {
        private static void Main(string[] args)
        {
            new AppRunner<ArgumentParser>().Run(args);
            Parsers1.Parse(args);
            
        }
    }
}


