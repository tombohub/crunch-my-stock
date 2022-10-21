using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Daily SPY ROI for the strategy accross all securities. Value is in %
    /// </summary>
    public partial class SpyRoi
    {
        public DateOnly? Date { get; set; }
        public decimal? SpyRoi1 { get; set; }
        public int? AreaId { get; set; }
    }
}
