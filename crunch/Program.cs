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


