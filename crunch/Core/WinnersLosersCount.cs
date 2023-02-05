namespace Crunch.Core
{
    /// <summary>
    /// Represents number of winning vs number of losing securities
    /// for the strategy.
    /// </summary>
    internal record WinnersLosersCount
    {
        public TradingDay TradingDay { get; init; }
        public SecurityType SecurityType { get; init; }
        /// <summary>
        /// Number of winning securities (positive gain)
        /// </summary>
        public int WinnersCount { get; init; }

        /// <summary>
        /// Number of losing securities(negative gain aka loss)
        /// </summary>
        public int LosersCount { get; init; }
    }
}