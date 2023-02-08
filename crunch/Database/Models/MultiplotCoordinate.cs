using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

/// <summary>
/// Coordinates  and size for each report plot so it can be draw properly inside the code and create multiplot image.
/// 
/// Coordinates x and y represen top left corner of the rectangle. Size is marked by &apos;width&apos; and &apos;height&apos;. width goes along x axis left to right and height along y axis top down.
/// </summary>
public partial class MultiplotCoordinate
{
    /// <summary>
    /// name of the strategy which also corresponds with Enum in application code
    /// </summary>
    public string Strategy { get; set; }

    /// <summary>
    /// name of the report which also corresponds with Enum in application code
    /// </summary>
    public string AreaName { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    /// <summary>
    /// true if report is included in multiplot
    /// </summary>
    public bool IsIncluded { get; set; }

    public int Id { get; set; }
}
