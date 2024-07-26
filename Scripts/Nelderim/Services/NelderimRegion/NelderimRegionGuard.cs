using System.Collections.Generic;

namespace Server.Nelderim;

internal class NelderimRegionGuard
{
    public string Name { get; set; }
    public double? Female { get; set; }
    public Dictionary<string, double> Population { get; set; }
}