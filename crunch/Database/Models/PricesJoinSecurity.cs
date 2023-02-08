using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

public partial class PricesJoinSecurity
{
    public DateOnly? Date { get; set; }

    public decimal? Open { get; set; }

    public decimal? Close { get; set; }

    public string Symbol { get; set; }

    public string Type { get; set; }

    public string Exchange { get; set; }

    public string Status { get; set; }

    public DateOnly? IpoDate { get; set; }

    public DateOnly? DelistingDate { get; set; }
}
