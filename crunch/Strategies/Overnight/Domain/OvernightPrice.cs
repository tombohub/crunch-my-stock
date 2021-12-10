namespace Crunch.Strategies.Overnight.Domain
{
    /// <summary>
    /// Represents prices for overnight.
    /// One night.
    /// </summary>
    record OvernightPrice
    {
        public string Symbol { get; set; }

        public double PreviousDayClose { get; set; }

        public double CurrentDayOpen { get; set; }

        public OvernightPrice(string symbol, double previousDayClose, double currentDayOpen)
        {
            Symbol = symbol;
            PreviousDayClose = previousDayClose;
            CurrentDayOpen = currentDayOpen;
        }
    }
}
