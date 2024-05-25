using System;
using System.Collections.Generic;
using System.Linq;

public class Capability
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Capability(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

public class GameSetup
{
    private List<Card> eventCards;
    private List<Card> propagandaCards;
    private Stack<Card> drawPile;
    private List<Card> discardPile;
    private List<Capability> britishCapabilities;
    private List<Capability> irgunCapabilities;
    private List<Area> areas;
    public string Initiative { get; private set; }
    public int PoliticalWill { get; private set; }
    public int HaganahTrack { get; private set; }

    public GameSetup()
    {
        eventCards = new List<Card>
        {
            new Card("Operation Agatha", "Event", "Black Saturday: British Search in 1 City then Mass Detention there. Shift Haganah Track 1 step to the left."),
            new Card("Exodus Affair", "Event", "Crackdown on immigration: Remove all Cells from Coastal Districts with Troops to Prison."),
            new Card("King David Hotel", "Event", "Widespread condemnation: Remove 1 Terror marker from the map to shift Haganah Track 1 step to the left."),
            // Add the remaining event cards here with their text
            new Card("Card4", "Event", "Event 4 Text"),
            new Card("Card5", "Event", "Event 5 Text"),
            new Card("Card6", "Event", "Event 6 Text"),
            new Card("Card7", "Event", "Event 7 Text"),
            new Card("Card8", "Event", "Event 8 Text"),
            new Card("Card9", "Event", "Event 9 Text"),
            new Card("Card10", "Event", "Event 10 Text"),
            new Card("Card11", "Event", "Event 11 Text"),
            new Card("Card12", "Event", "Event 12 Text"),
            new Card("Card13", "Event", "Event 13 Text"),
            new Card("Card14", "Event", "Event 14 Text"),
            new Card("Card15", "Event", "Event 15 Text"),
            new Card("Card16", "Event", "Event 16 Text"),
            new Card("Card17", "Event", "Event 17 Text"),
            new Card("Card18", "Event", "Event 18 Text"),
            new Card("Card19", "Event", "Event 19 Text"),
            new Card("Card20", "Event", "Event 20 Text")
        };

        propagandaCards = new List<Card>
        {
            new Card("Propaganda1", "Propaganda", "Propaganda 1 Text"),
            new Card("Propaganda2", "Propaganda", "Propaganda 2 Text"),
            new Card("Propaganda3", "Propaganda", "Propaganda 3 Text")
        };

        drawPile = new Stack<Card>();
        discardPile = new List<Card>();
        britishCapabilities = new List<Capability>();
        irgunCapabilities = new List<Capability>();

        // Initialize areas
        areas = new List<Area>
        {
            new Area("City1", 2, new List<string> { "District1", "Railway1" }),
            new Area("City2", 3, new List<string> { "District2", "Railway2" }),
            new Area("District1", 1, new List<string> { "City1", "District2" }),
            new Area("District2", 1, new List<string> { "City2", "District1" }),
            new Area("Railway1", 1, new List<string> { "City1", "Railway2" }),
            new Area("Railway2", 1, new List<string> { "City2", "Railway1" }),
            new Area("Prison", 0, new List<string>())
        };

        Initiative = "Irgun"; // Irgun starts first
        PoliticalWill = 18;   // Political Will starts at 18
        HaganahTrack = 4;     // Haganah Track starts at 4
    }

    public void SetupDeck()
    {
        // Shuffle event cards
        var random = new Random();
        var shuffledEventCards = eventCards.OrderBy(x => random.Next()).Take(18).ToList();

        // Divide into three piles of 6 cards each
        var pile1 = shuffledEventCards.Take(6).ToList();
        var pile2 = shuffledEventCards.Skip(6).Take(6).ToList();
        var pile3 = shuffledEventCards.Skip(12).Take(6).ToList();

        // Shuffle a Propaganda card with 2 of the Event cards and place these 3 cards under the remaining 4 Event cards
        pile1 = CreatePileWithPropaganda(pile1, propagandaCards[0]);
        pile2 = CreatePileWithPropaganda(pile2, propagandaCards[1]);
        pile3 = CreatePileWithPropaganda(pile3, propagandaCards[2]);

        // Combine piles into the draw pile
        var combinedDeck = new List<Card>(pile1);
        combinedDeck.AddRange(pile2);
        combinedDeck.AddRange(pile3);

        // Convert to stack for game use
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
        britishCapabilities.Add(capability);
    }

    public void AddIrgunCapability(Capability capability)
    {
        irgunCapabilities.Add(capability);
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
        return britishCapabilities;
    }

    public List<Capability> GetIrgunCapabilities()
    {
        return irgunCapabilities;
    }

    public List<Area> GetAreas()
    {
        return areas;
    }
}
