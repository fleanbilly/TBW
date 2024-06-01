using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using Windows.ApplicationModel.Contacts;

namespace TBW
{
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
        private Stack<IntelMarker> intelMarkers;
        private List<IntelMarker> britishIntelMarkers;
        private List<string> avaiableOptions = new List<string> { "Full Operation and Special Activity", "Take Event / Block Event", "Limited Operation" };
        public string Initiative { get; set; }
        public int PoliticalWill { get; set; }
        public int HaganahTrack { get; set; }
        public bool FullOperationTaken { get; set; }
        public bool EventTaken { get; set; }
        public bool LimitedOperationTaken { get; set; }

        //turn tracking
        public string CurrentTurnPlayer { get; set; } = "Irgun";

        public bool BritishTurnCompleted { get; set; }
        public bool IrgunTurnCompleted { get; set; }

        public int TurnNumber { get; private set; } = 1;


        public GameSetup()
        {
            InitializeIntelMarkers();

            // Initialize event cards
            eventCards = new List<Card>
        {

        new Card("P1 - Operation Agatha", "Event",
            "Option 1: Black Saturday: British Search in 1 City then Mass Detention there. Shift Haganah Track 1 step to the left.",
            "Option 2: Arrest moderate leaders: Shift Haganah Track 1 step to the right. If it is now at 4, Irgun may perform Sabotage in 1 space"),
        new Card("P2 - Exodus Affair*", "Event",
            "Option 1: Crackdown on immigration: Remove all Cells from Coastal Districts with Troops to Prison.",
            "Option 2: Lower Political Will by 3."),
        new Card("P3 - King David Hotel", "Event",
            "Option 1: Widespread condemnation: Remove 1 Terror marker from the map to shift Haganah Track 1 step to the left.",
            "Option 2: Place 2 Terror markers in 1 City with an Underground Cell."),
        new Card("P4 - Illegal Immigration*", "Event",
            "Option 1: British block entry: If no Sabotage markers in Coastal Districts,\nPolitical Will +2. Otherwise, lower Political Will by 1 for every \r\n2 Sabotage markers or Irgun pieces in Coastal Districts (max of –3 Political Will).",
            "Option 2: British block entry: If no Sabotage markers in Coastal Districts,\nPolitical Will +2. Otherwise, lower Political Will by 1 for every \r\n2 Sabotage markers or Irgun pieces in Coastal Districts (max of –3 Political Will)."),
        new Card("P5 - Acre Prison Break", "Event",
            "Option 1: Prisoners sent to Eritrea: Remove up to 2 Cells in Prison from the game.",
            "Option 2: Mass escape: Move all Cells in Prison to Available"),
        new Card("P6 - Montgomery", "Event",
            "Option 1: Pushes harsh measures: British may Search in up to 3 spaces but may not use Intel Chits.",
            "Option 2: Place 1 Cell in each City without Troops."),
        new Card("P7 - Lehi", "Event",
            "Option 1: Poor coordination: Remove up to 1 Cell to Available in each space with more than 1 Cell.",
            "Option 2: Assassinations: Remove up to 1 Police from each space with an Underground Cell."),
        new Card("P8 - Harry Truman*", "Event",
            "Option 1: Offers support: Increase Political Will by 2. British may Deploy in up to 3 spaces.",
            "Option 2: Pressures British: Lower Political Will by 1 for each Curfew on the map."),
        new Card("P10 - Palestine Police Force", "Event",
            "Option 1: Guard railways: Place up to all Available Police in any Railway spaces.",
            "Option 2: Poorly funded: Remove half Police on the map to Available (round down)"),
        new Card("P11 - Operation Shark", "Event",
            "Option 1: Crackdown on Irgun: Search then Assault in 1 City.\n Remove an Arms Cache within 1 space of the City (+1 PW if an Arms Cache removed).",
            "Option 2: Leaders slip through net: Travel from up to 2 spaces with Cells and Curfew"),
        new Card("P12 - Peter Bergson", "Event",
            "Option 1: British complaints: Remove 2 Arms Caches from map to Available (gain no Political Will). ",
            "Option 2: American fundraising: Place up to 2 Arms Caches total in any spaces with Cells."),
        new Card("P14 - Martial Law", "Event",
            "Option 1: Extensive searches: British may Search in up to 2 Cities then Mass Detention.",
            "Option 2: Angers populace: Place up to 1 Cell in each City with a Curfew."),
        new Card("P15 - Chaim Weizmann", "Event",
            "Option 1: Meets with British: Shift Haganah Track 1 step to the left and remove up to 1 Sabotage marker from the map.",
            "Option 2: Moderates ignored: Shift Haganah Track 1 step to the right. Irgun may perform Sabotage in 1 space."),
        new Card("P16 - Alan Cunningham", "Event",
            "Option 1: Restrained approach: British may conduct 2 Limited Operations then Restore.",
            "Option 2: Cells released: British must place 2 Cells from Prison into any Cities"),
        new Card("P19 - Rome Embassy Bombing", "Event",
            "Option 1: Plot foiled: Remove up to 2 Cells and 1 Arms Cache to Available (+1 Political Will if an Arms Cache is removed).",
            "Option 2: International attacks: Roll a die and lower Political Will by 2 if the result is greater than 2"),
        new Card("P20 - Indian Independence*", "Event",
            "Option 1: Strategic value of Palestine: Increase Political Will by 2 if no Railways Sabotaged.",
            "Option 2: Growing pressure: Lower Political Will by 2."),
        new Card("P21 - United Resistance Movement", "Event",
            "Option 1: Cooperation falls apart: Shift Haganah Track 1 step to the left. Remove up to 2 Cells to Available",
            "Option 2: Offensive against railways: Shift Haganah Track 1 step to the right. May perform Sabotage at each Railway."),
        new Card("P22 - Jewish Agency", "Event",
            "Option 1: Cooperate or condemn terror?: Shift Haganah Track 1 step in either direction. Then, Irgun may conduct a Limited Operation nor British may draw an Intel Chit.",
            "Option 2: Cooperate or condemn terror?: Shift Haganah Track 1 step in either direction. Then, Irgun may conduct a Limited Operation nor British may draw an Intel Chit."),
        new Card("P24 - Sergeants Affair", "Event",
            "Option 1: Backlash in Britain: Increase Political Will by 1 for each Terror marker on map (max of +3 Political Will)",
            "Option 2: Calls for withdrawal: Lower Political Will by 1 for each Terror marker on map (max of –3 Political Will)"),
       new Card("P25 - Menachem Begin", "Event",
            "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
            "Option 2: Igun leader: Propaganda lowers Political Will by 2 per selected space."),
       new Card("P26 - Veterans of WWII", "Event",
            "Option 1: Experienced in resistance and combat: Irgun may Sabotage in up to 3 spaces or British may Assault in up to 3 spaces. The Executing Faction may also conduct a valid Special Activity for that Operation.",
            "Option 2: Experienced in resistance and combat: Irgun may Sabotage in up to 3 spaces or British may Assault in up to 3 spaces. The Executing Faction may also conduct a valid Special Activity for that Operation."),
       new Card("P27 - American Loans*", "Event",
            "Option 1: Passed through Congress: Britain may conduct an Operation in up to 4 spaces.",
            "Option 2: Financial Dunkirk”: Political Will –1. Britain must Pass on the next card with no draw of an Intel Chit"),
      new Card("P28 - CID", "Event",
            "Option 1: Intelligence gathering: Gain 1 Intel Chit for each space with both Police and Cells.",
            "Option 2: Headquarters targeted: Irgun may perform Sabotage in 1 space with Police, and removes any Police there if successful."),
      new Card("P30 - Roy Farran", "Event",
            "Option 1: Undercover police: Remove 1 Cell to Available from each of up to 3 spaces with Police.",
            "Option 2: Controversial killing: Remove 2 Police to Available. Lower Political Will by 1."),
       new Card("P31 - Dov Gruner", "Event",
            "Option 1: Captured and sentenced: Remove a Cell from 1 space with Troops to Prison.",
            "Option 2: Galvanizes resistance: Place up to 3 Cells total from  available if present there or move from other areas into any Cities or Districts"),
    };
            // Initialize capability cards
            capabilityCards = new List<Card>
            {
        new Card("P9 - Ernest Bevin", "Capability",
            "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1",
            "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
        new Card("P13 - Palmach", "Capability",
            "Option 1: Preparing for next fight: Haganah Track 4 (Coordinate) no longer adds an additional space to Irgun Operations",
            "Option 2: Haganah’s elite: Haganah Track 3 (Support) also adds an additional space to Irgun Operations"),
        new Card("P17 - Bevingrads", "Capability",
            "Option 1: Fortified bases: Terror places max 1 Terror marker",
            "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell"),
        new Card("P18 - 6th Airborne", "Capability",
            "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
            "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
        new Card("P23 - Road Mines", "Capability",
            "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces",
            "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3. "),
        new Card("P25 - Menachem Begin", "Capability",
            "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
            "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space "),
        new Card("P29 - Curfews", "Capability",
            "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
            "Option 2: Harsh measures backfire: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
        new Card("P32 - Weapon Factories", "Capability",
            "Option 1: Accidental detonations: If Irgun Sabotage rolls a 1, remove the Activated Cell to Available",
            "Option 2: Homemade innovation: Rob always places an Arms Cache (no roll)")
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
        new Capability("P9 - Ernest Bevin", 
            "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1",
            "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
        new Capability("P13 - Palmach", 
            "Option 1: Preparing for next fight: Haganah Track 4 (Coordinate) no longer adds an additional space to Irgun Operations",
            "Option 2: Haganah’s elite: Haganah Track 3 (Support) also adds an additional space to Irgun Operations"),
        new Capability("P17 - Bevingrads", 
            "Option 1: Fortified bases: Terror places max 1 Terror marker",
            "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell"),
        new Capability("P18 - 6th Airborne",
            "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
            "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
        new Capability("P23 - Road Mines",
            "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces",
            "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3. "),
        new Capability("P25 - Menachem Begin",
            "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
            "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space "),
        new Capability("P29 - Curfews",
            "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
            "Option 2: Harsh measures backfire: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
        new Capability("P32 - Weapon Factories",
            "Option 1: Accidental detonations: If Irgun Sabotage rolls a 1, remove the Activated Cell to Available",
            "Option 2: Homemade innovation: Rob always places an Arms Cache (no roll)")
    };

            irgunCapabilities = new List<Capability>
       {
        new Capability("P9 - Ernest Bevin",
            "Option 1: Foreign Secretary: Lower the Political Will loss from all starred (*) Events by 1",
            "Option 2: Declining great power: British Operations limited to up to 2 rather than 3 spaces."),
        new Capability("P13 - Palmach",
            "Option 1: Preparing for next fight: Haganah Track 4 (Coordinate) no longer adds an additional space to Irgun Operations",
            "Option 2: Haganah’s elite: Haganah Track 3 (Support) also adds an additional space to Irgun Operations"),
        new Capability("P17 - Bevingrads",
            "Option 1: Fortified bases: Terror places max 1 Terror marker",
            "Option 2: Isolated from population: If Intel Chit not used, Search in Cities requires 4 Cubes to Activate a Cell"),
        new Capability("P18 - 6th Airborne",
            "Option 1: Seasoned veterans: Assault removes 1 Irgun piece per Troop.",
            "Option 2: Cells evade capture: Assault and Mass Detention remove all Cells to Available (not Prison)."),
        new Capability("P23 - Road Mines",
            "Option 1: Armored cars: Patrol may remove Cells in up to 2 selected spaces",
            "Option 2: Early IEDs: When Patrol moves Police to any space with a Sabotage marker, remove 1 Police to Available on a roll of 1-3. "),
        new Capability("P25 - Menachem Begin",
            "Option 1: Documents captured: Each Arms Cache removed with Assault adds 2 Political Will and draws 2 Intel Chits.",
            "Option 2: Irgun leader: Propaganda lowers Political Will by 2 per selected space "),
        new Capability("P29 - Curfews",
            "Option 1: Punitive response: At start of Reset, remove 1 Cell to Available from each space with a Curfew.",
            "Option 2: Harsh measures backfire: At start of Reset, Irgun may place up to 1 Cell from Available in each space with a Curfew."),
        new Capability("P32 - Weapon Factories",
            "Option 1: Accidental detonations: If Irgun Sabotage rolls a 1, remove the Activated Cell to Available",
            "Option 2: Homemade innovation: Rob always places an Arms Cache (no roll)")
    };

            // Initialize selected capabilities
            selectedBritishCapabilities = new List<Capability>();
            selectedIrgunCapabilities = new List<Capability>();

            // Initialize areas
            areas = new List<Area>
        {
            //Cities
            new Area("Haifa City", "City", 2, true, new List<string> { "Haifa District", "Railway Tel Aviv-Haifa" }),
            new Area("Tel Aviv City", "City", 2,true, new List<string> { "Lydda", "Railway Tel Aviv-Haifa","Railway Tel Aviv-Jerusalem","Railway Tel Aviv-Egypt" }),
            new Area("Jerusalem City", "City", 2,false, new List<string> { "Lydda", "Railway Tel Aviv-Jerusalem" }),
            //Districts
            new Area("Galilee", "District", 1,true, new List<string> { "Haifa District", "Samaria", "Railway Haifa-Syria" }),
            new Area("Haifa District", "District", 1,true, new List<string> { "Galilee", "Haifa City", "Samaria", "Railway Tel Aviv-Haifa" }),
            new Area("Samaria", "District", 1,true, new List<string> { "Galilee", "Haifa District", "Lydda", "Railway Haifa-Jerusalem","Jerusalem District" }),
            new Area("Lydda", "District", 1,true, new List<string> { "Samaria", "Tel Aviv City", "Jerusalem", "Railway Tel Aviv-Jerusalem","Railway Tel Aviv-Egypt" }),
            new Area("Jerusalem District", "District", 1,false, new List<string> { "Lydda", "Gaza", "Railway Tel Aviv-Jerusalem" }),
            new Area("Gaza", "District", 1,true, new List<string> { "Jerusalem District", "Lydda", "Railway Tel Aviv-Egypt" }),
        
            //Railways
            new Area("Railway Haifa-Syria", "Railway", 0,false, new List<string> { "Galilee", "Haifa District","Haifa City" }),
            new Area("Railway Tel Aviv-Haifa", "Railway", 0, false,new List<string> { "Haifa City","Haifa District", "Lydda", "Tel Aviv City" }),
            new Area("Railway Tel Aviv-Jerusalem", "Railway", 0,false, new List<string> { "Lydda","Jerusalem District","Jerusalem City","Tel Aviv City" }),
            new Area("Railway Tel Aviv-Egypt", "Railway", 0, false,new List<string> { "Gaza", "Lydda", "Tel Aviv City" }),
            //Holding Areas
            new Area("Prison", "Prison", 0, false,new List<string>()), // Prison holding area
            new Area("Available British", "Available", 0,false, new List<string>()), // Available British units
            new Area("Available Irgun", "Available", 0, false,new List<string>()) // Available Irgun units
        };

            Initiative = "Irgun"; // Irgun starts first
            PoliticalWill = 18;   // Political Will starts at 18
            HaganahTrack = 4;     // Haganah Track starts at 4

            FullOperationTaken = false;
            EventTaken = false;
            LimitedOperationTaken = false;
        }
        public void IncrementTurnNumber()
        {
            TurnNumber++;
        }
        public Card PeekAtTopCard()
        {
            return drawPile.Count > 0 ? drawPile.Peek() : null;
        }

        private void InitializeIntelMarkers()
        {
            var markers = new List<IntelMarker>
        {
            new IntelMarker(0),
            new IntelMarker(0),
            new IntelMarker(0),
            new IntelMarker(1),
            new IntelMarker(1),
            new IntelMarker(1),
            new IntelMarker(1),
            new IntelMarker(2),
            new IntelMarker(2),
            new IntelMarker(2),
            new IntelMarker(2),
        };

            var random = new Random();
            intelMarkers = new Stack<IntelMarker>(markers.OrderBy(m => random.Next()));
            britishIntelMarkers = new List<IntelMarker>();
        }

        public IntelMarker DrawIntelMarker()
        {
            if (intelMarkers.Count > 0)
            {
                var marker = intelMarkers.Pop();
                britishIntelMarkers.Add(marker);
                return marker;
            }
            return null;
        }

        public void ReturnIntelMarker(IntelMarker marker)
        {
            if (britishIntelMarkers.Contains(marker))
            {
                britishIntelMarkers.Remove(marker);
                intelMarkers.Push(marker);
            }
        }

        public List<IntelMarker> GetBritishIntelMarkers()
        {
            return new List<IntelMarker>(britishIntelMarkers);
        }

        public int GetIntelMarkerPoolCount()
        {
            return intelMarkers.Count;
        }

        public void ResetOptionStates()
        {
            FullOperationTaken = false;
            EventTaken = false;
            LimitedOperationTaken = false;
        }


        public void SetupDeck()
        {
            var random = new Random();
            var allCards = eventCards.Concat(capabilityCards).OrderBy(x => random.Next()).ToList(); // Combine and shuffle event and capability cards

            // Draw the first 18 cards to form the deck
            var drawDeck = allCards.Take(18).ToList();

            // Split the deck into three piles
            var pile1 = drawDeck.Take(6).ToList();
            var pile2 = drawDeck.Skip(6).Take(6).ToList();
            var pile3 = drawDeck.Skip(12).Take(6).ToList();

            // Add one propaganda card to the bottom of each pile
            pile1.Add(propagandaCards[0]);
            pile2.Add(propagandaCards[1]);
            pile3.Add(propagandaCards[2]);

            // Shuffle only the bottom 3 cards of each pile
            ShuffleBottomThree(pile1, random);
            ShuffleBottomThree(pile2, random);
            ShuffleBottomThree(pile3, random);

            // Combine all piles to form the final deck while preserving their order
            var combinedDeck = new List<Card>();
            combinedDeck.AddRange(pile1);
            combinedDeck.AddRange(pile2);
            combinedDeck.AddRange(pile3);

            drawPile = new Stack<Card>(combinedDeck.Reverse<Card>());
        }

        private void ShuffleBottomThree(List<Card> pile, Random random)
        {
            var top = pile.Take(4).ToList();
            var bottom = pile.Skip(4).OrderBy(x => random.Next()).ToList();
            pile.Clear();
            pile.AddRange(top);
            pile.AddRange(bottom);
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
        public void SetupStartingBoardState()
        {
            PoliticalWill = 18;
            HaganahTrack = 4;

            // Clear any existing units
            foreach (var area in areas)
            {
                area.Units.Clear();
            }

            // Irgun Units
            GetAreaByName("Galilee").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Haifa District").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Haifa City").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Samaria").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Tel Aviv City").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Lydda").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Lydda").AddUnit(new Unit("Weapons", "Irgun", "Active"), 1);
            GetAreaByName("Jerusalem District").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Jerusalem District").AddUnit(new Unit("Weapons", "Irgun", "Active"), 1);
            GetAreaByName("Jerusalem City").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);
            GetAreaByName("Gaza").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 1);


            // British Units
            GetAreaByName("Galilee").AddUnit(new Unit("Troop", "British", "Active"), 2);
            GetAreaByName("Haifa City").AddUnit(new Unit("Troop", "British", "Active"), 2);
            GetAreaByName("Haifa City").AddUnit(new Unit("Police", "British", "Active"), 1);
            GetAreaByName("Samaria").AddUnit(new Unit("Troop", "British", "Active"), 2);
            GetAreaByName("Tel Aviv City").AddUnit(new Unit("Police", "British", "Active"), 1);
            GetAreaByName("Tel Aviv City").AddUnit(new Unit("Troop", "British", "Active"), 2);
            GetAreaByName("Jerusalem City").AddUnit(new Unit("Troop", "British", "Active"), 2);
            GetAreaByName("Jerusalem City").AddUnit(new Unit("Police", "British", "Active"), 1);
            GetAreaByName("Gaza").AddUnit(new Unit("Troop", "British", "Active"), 2);


            // Assuming Available Irgun and Available British are holding areas for unused units
            GetAreaByName("Available Irgun").AddUnit(new Unit("Cell", "Irgun", "Hidden"), 6);
            GetAreaByName("Available Irgun").AddUnit(new Unit("Weapons", "Irgun", "Active"), 3);
            GetAreaByName("Available British").AddUnit(new Unit("Police", "British", "Active"), 3);

            // Example of adding markers
            //GetAreaByName("Haifa City").AddMarker("Sabotage");
            //GetAreaByName("Jerusalem City").AddMarker("Curfew");
            //GetAreaByName("Jerusalem City").AddMarker("Terror");
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
        public void AddUnitToArea(string areaName, Unit unit)
        {
            var area = GetAreaByName(areaName);
            if (area != null)
            {
                area.AddUnit(unit);
            }
        }

        public void RemoveUnitFromArea(string areaName, Unit unit)
        {
            var area = GetAreaByName(areaName);
            if (area != null)
            {
                var unitToRemove = area.Units.FirstOrDefault(u => u.Type == unit.Type && u.Faction == unit.Faction && u.State == unit.State);
                if (unitToRemove != null)
                {
                    area.RemoveUnit(unitToRemove);
                }
            }
        }

        public void MoveUnitBetweenAreas(string fromAreaName, string toAreaName, Unit unit)
        {
            var fromArea = GetAreaByName(fromAreaName);
            var toArea = GetAreaByName(toAreaName);

            if (fromArea != null && toArea != null)
            {
                var unitToMove = fromArea.Units.FirstOrDefault(u => u.Equals(unit));
                if (unitToMove != null)
                {
                    fromArea.RemoveUnit(unitToMove);
                    toArea.AddUnit(unitToMove);
                }
            }
        }

        public Array AvailableOptions(string action, string item = "")
        {
            if (action == "remove")
            {
                avaiableOptions.Remove(item);
                return avaiableOptions.ToArray();
            }
            else if(action == "reset" ){ 
                avaiableOptions = new List<string> { "Full Operation and Special Activity", "Take Event / Block Event", "Limited Operation" };
                return avaiableOptions.ToArray();
            }
            else
            {
                return avaiableOptions.ToArray();
                
            }
          
        }

    }
}
