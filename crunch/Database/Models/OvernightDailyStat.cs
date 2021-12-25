using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    public partial class OvernightDailyStat
    {
        public int Id { get; set; }
        /// <summary>
        /// Date of the strategy
        /// </summary>
        public DateOnly Date { get; set; }
        public string Symbol { get; set; }
        /// <summary>
        /// Previous trading day closing price
        /// </summary>
        public decimal StartPrice { get; set; }
        /// <summary>
        /// Strategy date opening price
        /// </summary>
        public decimal EndPrice { get; set; }
        /// <summary>
        /// Change between previous day close and today open price in %
        /// </summary>
        public decimal ChangePct { get; set; }
        public string Weekday { get; set; }
    }
}
