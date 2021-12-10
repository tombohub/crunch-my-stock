using CsvHelper.Configuration.Attributes;

namespace CrunchTests.OvernightStrategy
{
    /// <summary>
    /// Represents data model from the csv file
    /// </summary>
    class OvernightStatsCsvModel
    {
        [Name("symbol")]
        public string Symbol { get; set; }

        [Name("security_type")]
        public string SecurityType { get; set; }

        [Name("strategy")]
        public string Strategy { get; set; }

        [Name("return_on_initial_capital")]
        public double Roi { get; set; }
    }
}
