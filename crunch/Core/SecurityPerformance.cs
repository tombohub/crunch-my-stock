namespace Crunch.Core
{
    /// <summary>
    /// Represents single security performance over time.
    /// Performance is values as change in percent.
    /// </summary>
    internal record SecurityPerformance
    {
        /// <summary>
        /// Security symbol ticker
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Change in % between starting price and ending price
        /// </summary>
        public double ChangePct { get; set; }
    }
}