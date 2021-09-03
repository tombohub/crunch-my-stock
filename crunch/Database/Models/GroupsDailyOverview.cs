using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Database.Models
{
    public partial class GroupsDailyOverview
    {
        public DateTime Date { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public long Stocks { get; set; }
        public long MarketCap { get; set; }
        public double? Dividend { get; set; }
        public double? Pe { get; set; }
        public double? FwdPe { get; set; }
        public double? Peg { get; set; }
        public double? FloatShort { get; set; }
        public double Change { get; set; }
        public long Volume { get; set; }
        public DateTime CreatedAt { get; set; }
        public long Id { get; set; }
    }
}
