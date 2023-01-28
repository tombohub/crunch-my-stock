namespace Crunch.Core
{
    public record SecurityPrice
    {
        public TradingDay Date { get; init; }
        public Symbol Symbol { get; init; }
        public OHLC Price { get; init; }
        public uint Volume { get; init; }
    }
}