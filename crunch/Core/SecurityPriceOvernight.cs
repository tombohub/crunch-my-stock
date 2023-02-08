namespace Crunch.Core
{
    /// <summary>
    /// Represents overnight price
    /// </summary>
    public record SecurityPriceOvernight
    {
        public required Symbol Symbol { get; init; }
        public required SecurityType SecurityType { get; init; }
        public required TradingDay TradingDay { get; init; }
        public required OHLC OHLC { get; init; }
    }
}