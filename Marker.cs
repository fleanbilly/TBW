namespace TBW
{
    public class Marker
    {
        public string Type { get; set; }
        public string Effect { get; set; }

        public Marker(string type, string effect)
        {
            Type = type;
            Effect = effect;
        }
    }
}

