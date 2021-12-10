﻿using System;

namespace Crunch.Database.Models
{
    public partial class Crametorium
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Symbol { get; set; }
        public DateOnly PickDate { get; set; }
        public float StartPrice { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Close { get; set; }
        public float ChangeOpen { get; set; }
        public float ChangeHigh { get; set; }
        public float ChangeClose { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
