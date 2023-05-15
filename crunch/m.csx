#r "System"
using System;
var ts = "2021-02-12 00:00:00";
DateOnly d = DateOnly.Parse(ts);
Console.WriteLine(d);