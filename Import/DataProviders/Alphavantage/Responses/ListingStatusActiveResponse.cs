using CsvHelper.Configuration.Attributes;

namespace CrunchImport.DataProviders.Alphavantage.Responses
{
    internal class ListingStatusActiveResponse
    {
        [Name("symbol")]
        public string Symbol { get; set; }

        [Name("name")]
        public string Name { get; set; }

        [Name("exchange")]
        public string Exchange { get; set; }

        [Name("assetType")]
        public string AssetType { get; set; }

        [Name("ipoDate")]
        public DateOnly IpoDate { get; set; }

        [Name("status")]
        public string Status { get; set; }
    }
}