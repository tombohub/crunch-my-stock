using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommandDotNet;
using Crunch.CLI;

namespace Crunch
{
    class Program
    {
        private static void Main(string[] args)
        {
            new AppRunner<ArgumentParser>().Run(args);
        }
    }
}


