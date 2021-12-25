using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Calculates single measures and important metrics
    /// </summary>
    public partial class SingleMetric
    {
        public DateOnly? Date { get; set; }
        public decimal? AverageRoi { get; set; }
        public decimal? SpyRoi { get; set; }
    }
}
