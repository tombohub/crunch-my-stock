using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Crunch.Strategies.Overnight;

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
