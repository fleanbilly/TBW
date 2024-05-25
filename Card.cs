public class Card
{
    public string Title { get; set; }
    public string Type { get; set; } // "Event" or "Propaganda"
    public string Text { get; set; }

    public Card(string title, string type, string text)
    {
        Title = title;
        Type = type;
        Text = text;
    }
}


