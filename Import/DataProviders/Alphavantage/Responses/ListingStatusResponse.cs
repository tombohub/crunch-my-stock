namespace CrunchImport.DataProviders.Alphavantage.Responses
{
    internal class ListingStatusResponse
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string AssetType { get; set; }
        public DateOnly IpoDate { get; set; }
        public DateOnly DelistingDate { get; set; }
        public string Status { get; set; }
    }
}