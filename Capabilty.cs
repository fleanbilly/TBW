public class Capability
{
    public string Name { get; set; }
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public int SelectedOption { get; private set; } // Add this property

    public Capability(string name, string option1, string option2)
    {
        Name = name;
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




