namespace Crunch.Core
{
    public record SpyRoi
    {
        public required TradingDay TradingDay { get; init; }
        public required decimal Roi { get; init; }
    }
}