using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// Top 10 stocks each day according to performance(change_pct)
    /// </summary>
    public partial class Top10Stock
    {
        /// <summary>
        /// Date of the strategy
        /// </summary>
        public DateOnly? Date { get; set; }
        public string Symbol { get; set; }
        /// <summary>
        /// Previous trading day closing price
        /// </summary>
        public decimal? StartPrice { get; set; }
        /// <summary>
        /// Strategy date opening price
        /// </summary>
        public decimal? EndPrice { get; set; }
        /// <summary>
        /// Change between previous day close and today open price in %
        /// </summary>
        public decimal? ChangePct { get; set; }
        public long? Rank { get; set; }
        public int? AreaId { get; set; }
    }
}
