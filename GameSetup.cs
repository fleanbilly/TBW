using System;
using System.Collections.Generic;
using System.Linq;

public class GameSetup
{
    private List<Card> eventCards;
    private List<Card> capabilityCards;  
    private List<Card> propagandaCards;
    private Stack<Card> drawPile;
    private List<Card> discardPile;
    private List<Capability> britishCapabilities;
    private List<Capability> irgunCapabilities;
    private List<Capability> selectedBritishCapabilities;
    private List<Capability> selectedIrgunCapabilities;
    private List<Area> areas;
    public string Initiative { get; set; }
    public int PoliticalWill { get; set; }
    public int HaganahTrack { get; set; }

    public GameSetup()
    {
        // Initialize event cards
        eventCards = new List<Card>
        {
            new Card("Operation Agatha", "Event",
                "Option 1: British Search in 1 City then Mass Detention there. Shift Haganah Track 1 step to the left.",
                "Option 2: Shift Haganah Track 1 step to the right. If it is now at 4, Irgun may perform Sabotage in 1 space."),
            new Card("Exodus Affair", "Event",
                "Option 1: Crackdown on immigration: Remove all Cells from Coastal Districts with Troops to Prison.",
                "Option 2: Increase Political Will by 1."),
            new Card("King David Hotel", "Event",
                "Option 1: Widespread condemnation: Remove 1 Terror marker from the map to shift Haganah Track 1 step to the left.",
                "Option 2: Increase Political Will by 1."),
            new Card("Jewish Agency", "Event",
                "Option 1: Shift Haganah Track 1 step to the left. If Haganah Track is at 1 or higher, place 1 Cell in Available.",
                "Option 2: Shift Haganah Track 1 step to the right."),
            new Card("Dayr Yasin", "Event",
                "Option 1: Irgun Propagandize in up to 2 spaces. Add +1 for each space.",
                "Option 2: Irgun may Recruit in up to 3 spaces."),
            new Card("White Paper", "Event",
                "Option 1: Remove up to 2 Cells from the map to Available. Shift Haganah Track 1 step to the right.",
                "Option 2: Increase Political Will by 1."),
            new Card("Lehi", "Event",
                "Option 1: Irgun may Recruit in up to 3 spaces.",
                "Option 2: Irgun may place up to 1 Cell from Available in each space with a Curfew."),
            new Card("Sabotage Railways", "Event",
                "Option 1: Place Sabotage markers in up to 2 Railway spaces.",
                "Option 2: Irgun may place 1 Cell in Available for each Sabotage marker."),
            new Card("British Diplomacy", "Event",
                "Option 1: Shift Haganah Track 1 step to the left. If Haganah Track is at 1 or higher, place 1 Cell in Available.",
                "Option 2: Increase Political Will by 1."),
            new Card("Irgun Offensive", "Event",
                "Option 1: Place 1 Arms Cache in any space with an Irgun Cell.",
                "Option 2: Irgun may perform Sabotage in up to 2 spaces."),
            // Add capability cards
            new Card("Menachem Begin", "Capability",
                "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
                "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space."),
            new Card("Veterans of WWII", "Capability",
                "Option 1: Experienced in resistance and combat: Irgun may Sabotage in up to 3 spaces or British may Assault in up to 3 spaces.",
                "Option 2: Coordinated attack: The Executing Faction may also conduct a valid Special Activity for that Operation."),
            new Card("Alan Cunningham", "Capability",
                "Option 1: Restrained approach: British may conduct 2 Limited Operations then Restore. Cells released: British must place 2 Cells from Prison into any Cities.",
                "Option 2: Harsh measures: Remove 1 Cell to Available from each space with a British piece."),
            new Card("Curfews", "Capability",
                "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
                "Option 2: Harsh measures backfire: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
            new Card("Bevingrads", "Capability",
                "Option 1: Fortified bases: Terror places max 1 Terror marker.",
                "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell."),
            new Card("6th Airborne", "Capability",
                "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
                "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
            new Card("Ernest Bevin", "Capability",
                "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1.",
                "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
            new Card("Road Mines", "Capability",
                "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces.",
                "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3.")
        };
        // Initialize capability cards
        capabilityCards = new List<Card>
    {
        new Card("Menachem Begin", "Capability",
            "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
            "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space."),
        new Card("Veterans of WWII", "Capability",
            "Option 1: Experienced in resistance and combat: Irgun may Sabotage in up to 3 spaces or British may Assault in up to 3 spaces.",
            "Option 2: Coordinated attack: The Executing Faction may also conduct a valid Special Activity for that Operation."),
        new Card("Alan Cunningham", "Capability",
            "Option 1: Restrained approach: British may conduct 2 Limited Operations then Restore. Cells released: British must place 2 Cells from Prison into any Cities.",
            "Option 2: Harsh measures: Remove 1 Cell to Available from each space with a British piece."),
        new Card("Curfews", "Capability",
            "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
            "Option 2: Harsh measures backfire: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
        new Card("Bevingrads", "Capability",
            "Option 1: Fortified bases: Terror places max 1 Terror marker.",
            "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell."),
        new Card("6th Airborne", "Capability",
            "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
            "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
        new Card("Ernest Bevin", "Capability",
            "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1.",
            "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
        new Card("Road Mines", "Capability",
            "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces.",
            "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3.")
    };

        // Initialize propaganda cards
        propagandaCards = new List<Card>
        {
            new Card("Propaganda1", "Propaganda",
                "Option 1: Propaganda 1 Text",
                "Option 2: Alternative Propaganda 1 Text"),
            new Card("Propaganda2", "Propaganda",
                "Option 1: Propaganda 2 Text",
                "Option 2: Alternative Propaganda 2 Text"),
            new Card("Propaganda3", "Propaganda",
                "Option 1: Propaganda 3 Text",
                "Option 2: Alternative Propaganda 3 Text")
        };

        // Initialize draw and discard piles
        drawPile = new Stack<Card>();
        discardPile = new List<Card>();

        // Initialize capabilities
        britishCapabilities = new List<Capability>
        {
            new Capability("Alan Cunningham",
                "Option 1: Restrained approach: British may conduct 2 Limited Operations then Restore. Cells released: British must place 2 Cells from Prison into any Cities.",
                "Option 2: Harsh measures: Remove 1 Cell to Available from each space with a British piece."),
            new Capability("Curfews",
                "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
                "Option 2: Backlash: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
            new Capability("Bevingrads",
                "Option 1: Fortified bases: Terror places max 1 Terror marker.",
                "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell."),
            new Capability("6th Airborne",
                "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
                "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
            new Capability("Ernest Bevin",
                "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1.",
                "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
            new Capability("Road Mines",
                "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces.",
                "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3.")
        };

        irgunCapabilities = new List<Capability>
        {
            new Capability("Menachem Begin",
                "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
                "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space."),
            new Capability("Veterans of WWII",
                "Option 1: Experienced in resistance and combat: Irgun may Sabotage in up to 3 spaces or British may Assault in up to 3 spaces.",
                "Option 2: Coordinated attack: The Executing Faction may also conduct a valid Special Activity for that Operation."),
            new Capability("Harry Truman",
                "Option 1: Offers support: Increase Political Will by 2. British may Deploy in up to 3 spaces.",
                "Option 2: Pressures British: Lower Political Will by 1 for each Curfew on the map.")
        };

        // Initialize selected capabilities
        selectedBritishCapabilities = new List<Capability>();
        selectedIrgunCapabilities = new List<Capability>();

        // Initialize areas
        areas = new List<Area>
        {
            //Cities
            new Area("Haifa City", "City", 2, new List<string> { "Haifa District", "Railway Tel Aviv-Haifa" }),
            new Area("Tel Aviv City", "City", 2, new List<string> { "Lydda", "Railway Tel Aviv-Haifa","Railway Tel Aviv-Jerusalem","Railway Tel Aviv-Egypt" }),
            new Area("Jerusalem City", "City", 2, new List<string> { "Lydda", "Railway Tel Aviv-Jerusalem" }),
            //Districts
            new Area("Galilee", "District", 1, new List<string> { "Haifa District", "Samaria", "Railway Haifa-Syria" }),
            new Area("Haifa District", "District", 1, new List<string> { "Galilee", "Haifa City", "Samaria", "Railway Tel Aviv-Haifa" }),
            new Area("Samaria", "District", 1, new List<string> { "Galilee", "Haifa District", "Lydda", "Railway Haifa-Jerusalem","Jerusalem District" }),
            new Area("Lydda", "District", 1, new List<string> { "Samaria", "Tel Aviv City", "Jerusalem", "Railway Tel Aviv-Jerusalem","Railway Tel Aviv-Egypt" }),
            new Area("Jerusalem District", "District", 1, new List<string> { "Lydda", "Gaza", "Railway Tel Aviv-Jerusalem" }),
            new Area("Gaza", "District", 1, new List<string> { "Jerusalem District", "Lydda", "Railway Tel Aviv-Egypt" }),
        
            //Railways
            new Area("Railway Haifa-Syria", "Railway", 0, new List<string> { "Galilee", "Haifa District","Haifa City" }),
            new Area("Railway Tel Aviv-Haifa", "Railway", 0, new List<string> { "Haifa City","Haifa District", "Lydda", "Tel Aviv City" }),
            new Area("Railway Tel Aviv-Jerusalem", "Railway", 0, new List<string> { "Lydda","Jerusalem District","Jerusalem City","Tel Aviv City" }),
            new Area("Railway Tel Aviv-Egypt", "Railway", 0, new List<string> { "Gaza", "Lydda", "Tel Aviv City" }),
            //Holding Areas
            new Area("Prison", "Prison", 0, new List<string>()), // Prison holding area
            new Area("Available British", "Available", 0, new List<string>()), // Available British units
            new Area("Available Irgun", "Available", 0, new List<string>()) // Available Irgun units
        };

        Initiative = "Irgun"; // Irgun starts first
        PoliticalWill = 18;   // Political Will starts at 18
        HaganahTrack = 4;     // Haganah Track starts at 4
    }

    
public void SetupDeck()
    {
        var random = new Random();
        var shuffledEventCards = eventCards.OrderBy(x => random.Next()).Take(18).ToList();

        var pile1 = shuffledEventCards.Take(6).ToList();
        var pile2 = shuffledEventCards.Skip(6).Take(6).ToList();
        var pile3 = shuffledEventCards.Skip(12).Take(6).ToList();

        pile1 = CreatePileWithPropaganda(pile1, propagandaCards[0]);
        pile2 = CreatePileWithPropaganda(pile2, propagandaCards[1]);
        pile3 = CreatePileWithPropaganda(pile3, propagandaCards[2]);

        var combinedDeck = new List<Card>(pile1);
        combinedDeck.AddRange(pile2);
        combinedDeck.AddRange(pile3);

        drawPile = new Stack<Card>(combinedDeck);
    }



    private List<Card> CreatePileWithPropaganda(List<Card> pile, Card propaganda)
    {
        var random = new Random();
        var eventSubset = pile.Take(2).OrderBy(x => random.Next()).ToList();
        eventSubset.Add(propaganda);
        eventSubset = eventSubset.OrderBy(x => random.Next()).ToList();
        pile.RemoveRange(0, 2);
        pile.InsertRange(0, eventSubset);
        return pile;
    }

    public Card DrawCard()
    {
        if (drawPile.Count > 0)
        {
            var card = drawPile.Pop();
            discardPile.Add(card);
            return card;
        }
        return null; // No cards left in the draw pile
    }

    public void AddBritishCapability(Capability capability)
    {
        selectedBritishCapabilities.Add(capability);
    }

    public void AddIrgunCapability(Capability capability)
    {
        selectedIrgunCapabilities.Add(capability);
    }

    public Stack<Card> GetDrawPile()
    {
        return drawPile;
    }

    public List<Card> GetDiscardPile()
    {
        return discardPile;
    }

    public List<Capability> GetBritishCapabilities()
    {
        return selectedBritishCapabilities;
    }

    public List<Capability> GetIrgunCapabilities()
    {
        return selectedIrgunCapabilities;
    }

    public List<Area> GetAreas()
    {
        return areas;
    }

    public Area GetAreaByName(string name)
    {
        return areas.FirstOrDefault(a => a.Name == name);
    }

    public List<Capability> GetAllBritishCapabilities()
    {
        return britishCapabilities;
    }

    public List<Capability> GetAllIrgunCapabilities()
    {
        return irgunCapabilities;
    }
}
