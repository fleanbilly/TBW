public class Card
{
    public string Title { get; set; }
    public string Type { get; set; } // "Event", "Capability", or "Propaganda"
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public int SelectedOption { get; private set; }

    public Card(string title, string type, string option1, string option2)
    {
        Title = title;
        Type = type;
        Option1 = option1;
        Option2 = option2;
        SelectedOption = 0; // No option selected by default
    }

    public void SelectOption(int option)
    {
        if (option == 1 || option == 2)
        {
            SelectedOption = option;
        }
    }
}






