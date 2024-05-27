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

    public void AddUnit(Unit unit, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            Units.Add(new Unit(unit.Type, unit.Faction, unit.State));
        }
    }


    public void RemoveUnit(Unit unit)
    {
        var unitToRemove = Units.FirstOrDefault(u => u.Equals(unit));
        if (unitToRemove != null)
        {
            Units.Remove(unitToRemove);
        }
    }


}




