using Crunch.Domain;

namespace Crunch.Strategies.Overnight.Reports
{
    /// <summary>
    /// Report data model for Winners and Losers count
    /// </summary>
    public record WinnersLosersRatioReport
    {
        public SecurityType SecurityType { get; init; }
        public int WinnersCount { get; init; }
        public int LosersCount { get; init; }
    }
}
