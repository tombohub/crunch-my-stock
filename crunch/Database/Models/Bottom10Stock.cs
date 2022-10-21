using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    public partial class Bottom10Stock
    {
        public DateOnly? Date { get; set; }
        public string Symbol { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public decimal? ChangePct { get; set; }
        public long? Rank { get; set; }
        public int? AreaId { get; set; }
    }
}
