using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Database.Models
{
    public partial class Group
    {
        public long Id { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
    }
}
