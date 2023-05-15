
using System;

namespace Crunch
{
    /// <summary>
    /// DTO for data provider object to deliver in this format
    /// </summary>
    public record DataProviderDailyPriceDataDTO
    {
        public required string Symbol { get; init; }
        public required DateOnly Date { get; init; }
        public required decimal Open { get; init; }
        public required decimal High { get; init; }
        public required decimal Low { get; init; }
        public required decimal Close { get; init; }
        public required uint Volume { get; init; }
    }
}
