﻿public class Card
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Option1 { get; set; }
    public string Option2 { get; set; }

    public Card(string name, string type, string option1, string option2)
    {
        Name = name;
        Type = type;
        Option1 = option1;
        Option2 = option2;
    }
}





