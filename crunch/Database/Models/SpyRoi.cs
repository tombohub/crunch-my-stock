using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

/// <summary>
/// SPY roi (pct change) for a benchmark
/// </summary>
public partial class SpyRoi
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public decimal Roi { get; set; }
}
