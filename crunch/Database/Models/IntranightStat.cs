using System;

namespace Crunch.Database.Models
{
    public partial class IntranightStat
    {
        public long Id { get; set; }
        public DateOnly Date { get; set; }
        public string Symbol { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public double MaxPrice { get; set; }
        public TimeOnly MaxPriceTime { get; set; }
        public double MaxPriceChange { get; set; }
        public double MinPrice { get; set; }
        public TimeOnly MinPriceTime { get; set; }
        public double MinPriceChange { get; set; }
        public double DayVolume { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
