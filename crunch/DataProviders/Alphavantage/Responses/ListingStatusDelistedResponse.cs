using System;
using CsvHelper.Configuration.Attributes;

namespace Crunch.DataProviders.Alphavantage.Responses
{
    internal class ListingStatusDelistedResponse
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

        [Name("delistingDate")]
        public DateOnly DelistingDate { get; set; }

        [Name("status")]
        public string Status { get; set; }
    }
}