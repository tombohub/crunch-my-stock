namespace Crunch.Core
{
    public record SecurityPrice
    {
        public required Symbol Symbol { get; init; }
        public SecurityType SecurityType { get; init; }
        public required TradingDay TradingDay { get; init; }
        public required OHLC OHLC { get; init; }
        public required long Volume { get; init; }
    }
}