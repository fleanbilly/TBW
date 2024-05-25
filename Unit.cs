public class Unit
{
    public string Type { get; set; } // "British" or "Irgun"
    public string State { get; set; } // "Active", "Underground" (for Irgun), "Hidden"

    public Unit(string type, string state)
    {
        Type = type;
        State = state;
    }
}

