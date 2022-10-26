namespace CrunchImport
{
    /// <summary>
    /// Flat price data object for the symbol OHCL price on specific day
    /// </summary>
    internal record DailyPriceDTO
    {
        public DateOnly Date { get; init; }
        public string Symbol { get; init; }
        public decimal Open { get; init; }
        public decimal High { get; init; }
        public decimal Low { get; init; }
        public decimal Close { get; init; }
        public uint Volume { get; init; }
    }


}