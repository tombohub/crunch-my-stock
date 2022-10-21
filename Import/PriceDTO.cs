namespace Import
{
    /// <summary>
    /// Flat price data object for the symbol OHCL price on specific day
    /// </summary>
    internal record DailyPriceDTO
    {
        public DateOnly Date { get; set; }
        public string Symbol { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public uint Volume { get; set; }
    }


}