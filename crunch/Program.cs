using CommandDotNet;
using CommandDotNet.DataAnnotations;
using CommandDotNet.NameCasing;

namespace Crunch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new AppRunner<CommandLineParser>()
                .UseNameCasing(Case.LowerCase)
                .UseDataAnnotationValidations()
                .Run(args);
        }
    }
}