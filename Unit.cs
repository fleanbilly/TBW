public class Unit
{
    public string Type { get; set; }
    public string Faction { get; set; }
    public string State { get; set; }

    public Unit(string type, string faction, string state)
    {
        Type = type;
        Faction = faction;
        State = state;
    }

    public override bool Equals(object obj)
    {
        if (obj is Unit other)
        {
            return Type == other.Type && Faction == other.Faction && State == other.State;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Faction, State);
    }

    public override string ToString()
    {
        return $"{Type} ({Faction}, {State})";
    }
}












