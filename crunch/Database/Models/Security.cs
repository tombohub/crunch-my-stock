using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Database.Models
{
    public partial class Security
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
    }
}
