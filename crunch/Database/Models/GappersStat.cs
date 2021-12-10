using System;

namespace Crunch.Database.Models
{
    public partial class GappersStat
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public DateOnly Date { get; set; }
        public double PrevDayClose { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double GapPct { get; set; }
        public double HalfHourPct { get; set; }
        public long IsGapUp { get; set; }
        public long IsGain { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
