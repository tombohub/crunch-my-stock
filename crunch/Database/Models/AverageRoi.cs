using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

/// <summary>
/// Daily average overnight ROI for the strategy accross all securities. Value is in %
/// </summary>
public partial class AverageRoi
{
    public long Id { get; set; }

    public DateOnly Date { get; set; }

    public decimal AverageRoi1 { get; set; }
}
