using System;
using System.Collections.Generic;
using Crunch.Database.Models;

namespace Crunch.Database.Models
{
    public partial class PricesDailyOvernight
    {
        public int Id { get; set; }
        /// <summary>
        /// TradingDay of the strategy
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Previous trading day closing price
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// Strategy date opening price
        /// </summary>
        public decimal Close { get; set; }
        public int SecurityId { get; set; }

        public virtual Security Security { get; set; }
    }
}
