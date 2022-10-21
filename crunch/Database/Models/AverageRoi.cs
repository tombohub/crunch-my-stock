using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Daily average ROI for the strategy accross all securities. Value is in %
    /// </summary>
    public partial class AverageRoi
    {
        public DateOnly? Date { get; set; }
        public decimal? AverageRoi1 { get; set; }
        public int? AreaId { get; set; }
    }
}
