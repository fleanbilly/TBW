using System.Collections.Generic;

public class Area
{
    public string Name { get; set; }
    public string Type { get; set; } // "City", "District", "Railway", "Prison", "Available"
    public int PointValue { get; set; }
    public List<string> AdjacentAreas { get; set; }
    public List<Unit> Units { get; set; }

    public Area(string name, string type, int pointValue, List<string> adjacentAreas)
    {
        Name = name;
        Type = type;
        PointValue = pointValue;
        AdjacentAreas = adjacentAreas;
        Units = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }
}



