using CommandDotNet;
using CommandDotNet.NameCasing;

namespace CrunchImport
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new AppRunner<CommandLineParser>()
                .UseNameCasing(Case.LowerCase)
                .Run(args);
        }
    }
}