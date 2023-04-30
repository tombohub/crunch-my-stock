using CommandDotNet;
using CommandDotNet.DataAnnotations;
using CommandDotNet.NameCasing;


namespace ImportPrices
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new AppRunner<CommandLineParser>()
                .UseNameCasing(Case.LowerCase)
                .UseDataAnnotationValidations()
                .UseParseDirective()
                .Run(args);
        }
    }
}