using Crunch.Domain;

namespace Crunch.Strategies.Overnight
{
    /// <summary>
    /// Stats for single security symbol
    /// </summary>
    public record SingleSymbolStats
    (
        /// <summary>
        /// Symbol name, ex: MSFT
        /// </summary>
        string Symbol,

        /// <summary>
        /// Security type
        /// </summary>
        SecurityType SecurityType,

        /// <summary>
        /// Roi of the strategy
        /// </summary>
        double OvernightRoi,

        /// <summary>
        /// Roi if the benchmark (buy and hold)
        /// </summary>
        double BenchmarkRoi
    );
}
