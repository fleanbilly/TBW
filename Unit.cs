public class Unit
{
    public string Type { get; set; } // e.g., "Irgun", "British"
    public string Status { get; set; } // e.g., "Active", "Hidden"

    public Unit(string type, string status)
    {
        Type = type;
        Status = status;
    }
}



