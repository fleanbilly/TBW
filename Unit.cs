public class Unit
{
    public string Type { get; set; } // Example: "British", "Irgun"
    public string State { get; set; } // Example: "Hidden", "Active", etc.

    public Unit(string type, string state)
    {
        Type = type;
        State = state;
    }
}




