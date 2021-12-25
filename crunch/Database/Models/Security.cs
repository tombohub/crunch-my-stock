using System;
using System.Collections.Generic;

namespace Crunch.Database.Models
{
    /// <summary>
    /// List of available securities on market. Stocks and ETFs
    /// </summary>
    public partial class Security
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
}
