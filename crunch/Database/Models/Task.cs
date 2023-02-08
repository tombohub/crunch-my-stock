using System;
using System.Collections.Generic;

namespace Crunch.Database.Models;

/// <summary>
/// Tasks to be run periodically
/// </summary>
public partial class Task
{
    public int Id { get; set; }

    public string Task1 { get; set; }

    public string Description { get; set; }

    public TimeSpan Frequency { get; set; }
}
