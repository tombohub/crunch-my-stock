﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Database.Models
{
    public partial class Prices30m
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
        public string Interval { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}