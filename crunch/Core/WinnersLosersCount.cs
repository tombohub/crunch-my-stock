namespace Crunch.Core
{
    /// <summary>
    /// Represents number of winning vs number of losing securities
    /// for the strategy.
    /// </summary>
    internal record WinnersLosersCount
    {
        /// <summary>
        /// Number of winning securities (positive gain)
        /// </summary>
        public int WinnersCount { get; set; }

        /// <summary>
        /// Number of losing securities(negative gain aka loss)
        /// </summary>
        public int LosersCount { get; set; }
    }
}