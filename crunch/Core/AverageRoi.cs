namespace Crunch.Core
{
    public record AverageRoi
    {
        public required TradingDay TradingDay { get; init; }
        public required decimal Roi { get; init; }
        public required SecurityType SecurityType { get; init; }
    }
}