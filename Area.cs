using System.Collections.Generic;
using System.Linq;

namespace TBW
{
    public class Area
    {
        public string Name { get; set; }
        public string Type { get; set; } // "City", "District", "Railway", "Prison", "Available"
        public int PointValue { get; set; }
        public List<string> AdjacentAreas { get; set; }
        public List<Unit> Units { get; set; }
        public List<Marker> Markers { get; set; } // Updated to use Marker class

        public Area(string name, string type, int pointValue, List<string> adjacentAreas)
        {
            Name = name;
            Type = type;
            PointValue = pointValue;
            AdjacentAreas = adjacentAreas;
            Units = new List<Unit>();
            Markers = new List<Marker>(); // Initialize markers list
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

        // Method to add a marker
        public void AddMarker(string type, string effect = null)
        {   
            if(effect == null)
            {
                if(type == "Sabotage")
                {
                    effect = "If on the board when a Propaganda is flipped will lower Politcal Will ";
                }
                else if (type == "Terror")
                {
                    effect = "If on the board when a Propaganda is flipped will lower Politcal Will, it is twice as effective as Sabotage and should be a British priority to remove.";
                }
                else if(type == "Curfew")
                {
                    effect = "A side effect of British Operations, but can be exploited by Irgun with Propagandize to lower Political Will ";
                } else
                    {
                    effect = "No effect";
                }
            }
            Markers.Add(new Marker(type, effect));
        }

        // Method to remove a marker
        public void RemoveMarker(string type)
        {
            var markerToRemove = Markers.FirstOrDefault(m => m.Type == type);
            if (markerToRemove != null)
            {
                Markers.Remove(markerToRemove);
            }
        }
    }
}
