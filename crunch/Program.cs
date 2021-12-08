using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Crunch.UseCases;
using CommandLine;
//using CommandDotNet;
using Crunch.Domain;
using Crunch.Strategies;
using Crunch.Strategies.Overnight.CliControllers;
using Crunch.CLI.Controllers;
using Crunch.CLI;

namespace Crunch
{
    class Program
    {
        

        //[Command(Description ="Perform calculations")]
        //public class Calculator
        //{
        //    public void PriceTest(
        //        [Option("interval")]
        //        PriceInterval interval
        //        )
        //    {
        //        Console.WriteLine(interval);
        //    }
        //    [Command(Description = "Adds two numbers")]
        //    public void Add(int value1, int value2)
        //    {
        //        Console.WriteLine($"Answer:  {value1 + value2}");
        //    }

        //    [Command(Description = "Subtracts two numbers")]
        //    public void Subtract(int value1, int value2)
        //    {
        //        Console.WriteLine($"Answer:  {value1 - value2}");
        //    }
        //}


        private static void Main(string[] args)
        {
            //new AppRunner<Calculator>().Run(args);
            Parsers1.Parse(args);
            
        }
    }
}


