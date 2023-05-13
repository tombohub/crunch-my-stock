using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

/// <summary>
/// List of available securities on market. Stocks and ETFs
/// </summary>
public partial class Security
{
    public string Symbol { get; set; }

    public string Type { get; set; }

    public string Exchange { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int Id { get; set; }

    /// <summary>
    /// active or delisted
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// date of initial public offering
    /// </summary>
    public DateOnly IpoDate { get; set; }

    /// <summary>
    /// date when security was delisted, if delisted
    /// </summary>
    public DateOnly? DelistingDate { get; set; }

    public virtual ICollection<AverageRoi> AverageRois { get; set; } = new List<AverageRoi>();

    public virtual ICollection<DailyOvernightPerformance> DailyOvernightPerformances { get; set; } = new List<DailyOvernightPerformance>();

    public virtual ICollection<PricesDaily> PricesDailies { get; set; } = new List<PricesDaily>();
}
