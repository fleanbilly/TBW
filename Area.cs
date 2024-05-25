using System.Collections.Generic;

public class Area
{
    public string Name { get; set; }
    public int PointValue { get; set; }
    public List<string> AdjacentAreas { get; set; }
    public List<Unit> Units { get; set; }

    public Area(string name, int pointValue, List<string> adjacentAreas)
    {
        Name = name;
        PointValue = pointValue;
        AdjacentAreas = adjacentAreas;
        Units = new List<Unit>();
    }
}

