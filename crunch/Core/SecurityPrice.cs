namespace Crunch.Core
{
    public record SecurityPrice
    {
        public Symbol Symbol { get; init; }
        public TradingDay Date { get; init; }
        public OHLC Price { get; init; }
        public uint Volume { get; init; }
    }
}