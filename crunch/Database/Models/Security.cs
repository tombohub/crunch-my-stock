using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    public partial class Security
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
    }
}
