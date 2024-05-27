using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace TBW
{
    public partial class MainWindow : Window
    {
        private GameSetup gameSetup;
        private Card currentCard;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            gameSetup = new GameSetup();
            gameSetup.SetupDeck();
            gameSetup.SetupStartingBoardState(); 
            DisplayNextCard();
            UpdateBoardState();
        }
        private void DrawIntelMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            var marker = gameSetup.DrawIntelMarker();
            if (marker != null)
            {
                MessageBox.Show($"Drew Intel Marker with value: {marker.Value}");
            }
            else
            {
                MessageBox.Show("No more Intel Markers to draw.");
            }
            UpdateBoardState();
        }
        private void ReturnSpecificIntelMarker()
        {
            var markers = gameSetup.GetBritishIntelMarkers();
            if (markers.Any())
            {
                var markerValues = markers.Select(m => m.Value).ToList();
                var input = Microsoft.VisualBasic.Interaction.InputBox("Enter the value of the Intel Marker to return:", "Return Intel Marker", markerValues[0].ToString());

                if (int.TryParse(input, out int value) && markerValues.Contains(value))
                {
                    var marker = markers.FirstOrDefault(m => m.Value == value);
                    if (marker != null)
                    {
                        gameSetup.ReturnIntelMarker(marker);
                        MessageBox.Show($"Returned Intel Marker with value: {marker.Value}");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid value entered or marker not available.");
                }
            }
            else
            {
                MessageBox.Show("No Intel Markers to return.");
            }
            UpdateBoardState();
        }

        private void ReturnIntelMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnSpecificIntelMarker();
        }
        private void OpenManageUnitsWindow_Click(object sender, RoutedEventArgs e)
        {
            ManageUnitsWindow manageUnitsWindow = new ManageUnitsWindow(gameSetup,this);
            manageUnitsWindow.Owner = this;
            manageUnitsWindow.ShowDialog();
            UpdateBoardState(); // Refresh the board state after managing units
        }

        private void DisplayNextCard()
        {
            currentCard = gameSetup.DrawCard();
            if (currentCard != null)
            {
                cardTitleTextBlock.Text = $"{currentCard.Title} ({currentCard.Type})";
                cardOption1TextBlock.Text = $"Option 1: {currentCard.Option1}";
                cardOption2TextBlock.Text = $"Option 2: {currentCard.Option2}";
                cardSelectedOptionTextBlock.Text = "Selected Option: None";

                selectOption1Button.IsEnabled = true;
                selectOption2Button.IsEnabled = true;

                UpdateBoardState();
            }
            else
            {
                MessageBox.Show("No more cards in the draw pile.");
            }
        }

        //private void UpdateBoardState(string selectedOption = "")
        //{
        //    boardTextBox.Text = "Board State and Scoring Information\n";
        //    boardTextBox.Text += $"Current Player: {gameSetup.Initiative}\n";
        //    boardTextBox.Text += $"Political Will: {gameSetup.PoliticalWill}\n";
        //    boardTextBox.Text += $"Haganah Track: {gameSetup.HaganahTrack}\n\n";

        //    var intelMarkers = gameSetup.GetBritishIntelMarkers();
        //    boardTextBox.Text = $"British Intel Markers: {string.Join(", ", intelMarkers.Select(m => m.Value))}\n";
        //    boardTextBox.Text += $"Intel Marker Pool Count: {gameSetup.GetIntelMarkerPoolCount()}\n";


        //    if (!string.IsNullOrEmpty(selectedOption))
        //    {
        //        var availableOptions = new List<string> { "Full Operation and Special Activity", "Take Event / Block Event", "Limited Operation" };
        //        availableOptions.Remove(selectedOption);
        //        boardTextBox.Text += $"Selected Option: {selectedOption}\n";
        //        boardTextBox.Text += $"Available Options: {string.Join(", ", availableOptions)}\n\n";
        //    }

        //    boardTextBox.Text += "Selected Capabilities:\n";
        //    boardTextBox.Text += string.Join("\n", gameSetup.GetBritishCapabilities().Select(c => $"{c.Name}: {GetSelectedOptionText(c)}"));
        //    boardTextBox.Text += "\n";
        //    boardTextBox.Text += string.Join("\n", gameSetup.GetIrgunCapabilities().Select(c => $"{c.Name}: {GetSelectedOptionText(c)}"));
        //    boardTextBox.Text += "\n\n";

        //    foreach (var area in gameSetup.GetAreas())
        //    {
        //        boardTextBox.Text += $"{area.Name} ({area.Type}) - Points: {area.PointValue}\n";
        //        if (area.Name != "Available British")
        //        {
        //            boardTextBox.Text += $"Irgun: Cells: {area.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun")} - {area.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun" && u.State == "Hidden")} hidden, Weapons: {area.Units.Count(u => u.Type == "Weapon" && u.Faction == "Irgun")}\n";
        //        }
        //        if (area.Name != "Available Irgun")
        //        {
        //            boardTextBox.Text += $"British: Police: {area.Units.Count(u => u.Type == "Police" && u.Faction == "British")} - Troops: {area.Units.Count(u => u.Type == "Troop" && u.Faction == "British")}\n";
        //        }
        //        boardTextBox.Text += $"Markers: {string.Join(", ", area.Markers)}\n"; // Add this line to display markers
        //        boardTextBox.Text += "\n";
        //    }

        //    initiativeToggleButton.Content = gameSetup.Initiative == "Irgun" ? "Initiative: Irgun" : "Initiative: British";
        //    politicalWillTextBlock.Text = gameSetup.PoliticalWill.ToString();
        //    haganahTrackTextBlock.Text = gameSetup.HaganahTrack.ToString();

        //    drawDeckTextBlock.Text = $"Draw Deck: {gameSetup.GetDrawPile().Count} cards";
        //    discardDeckTextBlock.Text = $"Discard Pile: {gameSetup.GetDiscardPile().Count} cards";
        //}
        private void UpdateBoardState(string selectedOption = "")
        {
            boardTextBox.Text = "Board State and Scoring Information\n";
            boardTextBox.Text += $"Current Player: {gameSetup.Initiative}\n";
            boardTextBox.Text += $"Political Will: {gameSetup.PoliticalWill}\n";
            boardTextBox.Text += $"Haganah Track: {gameSetup.HaganahTrack}\n\n";

            if (!string.IsNullOrEmpty(selectedOption))
            {
                var availableOptions = new List<string> { "Full Operation and Special Activity", "Take Event / Block Event", "Limited Operation" };
                availableOptions.Remove(selectedOption);
                boardTextBox.Text += $"Selected Option: {selectedOption}\n";
                boardTextBox.Text += $"Available Options: {string.Join(", ", availableOptions)}\n\n";
            }

            boardTextBox.Text += "Selected Capabilities:\n";
            boardTextBox.Text += string.Join("\n", gameSetup.GetBritishCapabilities().Select(c => $"{c.Name}: {GetSelectedOptionText(c)}"));
            boardTextBox.Text += "\n";
            boardTextBox.Text += string.Join("\n", gameSetup.GetIrgunCapabilities().Select(c => $"{c.Name}: {GetSelectedOptionText(c)}"));
            boardTextBox.Text += "\n\n";

            foreach (var area in gameSetup.GetAreas())
            {
                boardTextBox.Text += $"{area.Name} ({area.Type}) - Points: {area.PointValue}\n";
                if (area.Name != "Available British")
                {
                    boardTextBox.Text += $"Irgun: Cells: {area.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun")} - {area.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun" && u.State == "Hidden")} hidden, Weapons: {area.Units.Count(u => u.Type == "Weapon" && u.Faction == "Irgun")}\n";
                }
                if (area.Name != "Available Irgun")
                {
                    boardTextBox.Text += $"British: Police: {area.Units.Count(u => u.Type == "Police" && u.Faction == "British")} - Troops: {area.Units.Count(u => u.Type == "Troop" && u.Faction == "British")}\n";
                }
                boardTextBox.Text += $"Markers: {string.Join(", ", area.Markers.Select(m => m.Type))}\n"; // Updated to show marker types
                boardTextBox.Text += "\n";
            }

            initiativeToggleButton.Content = gameSetup.Initiative == "Irgun" ? "Initiative: Irgun" : "Initiative: British";
            politicalWillTextBlock.Text = gameSetup.PoliticalWill.ToString();
            haganahTrackTextBlock.Text = gameSetup.HaganahTrack.ToString();

            drawDeckTextBlock.Text = $"Draw Deck: {gameSetup.GetDrawPile().Count} cards";
            discardDeckTextBlock.Text = $"Discard Pile: {gameSetup.GetDiscardPile().Count} cards";
        }


        public void RefreshBoardState()
        {
            UpdateBoardState();
        }






    

        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayNextCard();
        }

        private void ShowDiscardPileButton_Click(object sender, RoutedEventArgs e)
        {
            var discardPile = gameSetup.GetDiscardPile();
            string discardPileText = "Discard Pile:\n" + string.Join("\n", discardPile.Select(c => c.Title));
            MessageBox.Show(discardPileText, $"Discard Pile ({discardPile.Count} cards played)");
        }

        private void ShowDrawPileButton_Click(object sender, RoutedEventArgs e)
        {
            var drawPile = gameSetup.GetDrawPile();
            MessageBox.Show($"There are {drawPile.Count} cards left to play.", $"Draw Pile ({drawPile.Count} cards left to play)");
        }

        private void SelectOption1Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCard != null)
            {
                currentCard.SelectOption(1);
                cardSelectedOptionTextBlock.Text = "Selected Option: 1";
                selectOption1Button.IsEnabled = false;
                selectOption2Button.IsEnabled = false;

                if (currentCard.Type == "Capability")
                {
                    var capability = gameSetup.GetAllBritishCapabilities().FirstOrDefault(c => c.Name == currentCard.Title) ??
                                     gameSetup.GetAllIrgunCapabilities().FirstOrDefault(c => c.Name == currentCard.Title);

                    if (capability != null)
                    {
                        capability.SelectOption(1);
                        gameSetup.AddBritishCapability(capability); // Add to the selected list
                        UpdateBoardState();
                    }
                }
                else
                {
                    UpdateBoardState();
                }
            }
        }

        private void SelectOption2Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCard != null)
            {
                currentCard.SelectOption(2);
                cardSelectedOptionTextBlock.Text = "Selected Option: 2";
                selectOption1Button.IsEnabled = false;
                selectOption2Button.IsEnabled = false;

                if (currentCard.Type == "Capability")
                {
                    var capability = gameSetup.GetAllBritishCapabilities().FirstOrDefault(c => c.Name == currentCard.Title) ??
                                     gameSetup.GetAllIrgunCapabilities().FirstOrDefault(c => c.Name == currentCard.Title);

                    if (capability != null)
                    {
                        capability.SelectOption(2);
                        gameSetup.AddBritishCapability(capability); // Add to the selected list
                        UpdateBoardState();
                    }
                }
                else
                {
                    UpdateBoardState();
                }
            }
        }

        private void InitiativeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "Irgun";
                initiativeToggleButton.Content = "Initiative: Irgun";
                UpdateBoardState();
            }
        }

        private void InitiativeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "British";
                initiativeToggleButton.Content = "Initiative: British";
                UpdateBoardState();
            }
        }

        private void IncreasePoliticalWill_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.PoliticalWill < 20)
            {
                gameSetup.PoliticalWill++;
                UpdateBoardState();
            }
        }

        private void DecreasePoliticalWill_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.PoliticalWill > 0) // Ensure Political Will doesn't go below 0
            {
                gameSetup.PoliticalWill--;
                UpdateBoardState();
            }
        }

        private void IncreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.HaganahTrack < 4)
            {
                gameSetup.HaganahTrack++;
                UpdateBoardState();
            }
        }

        private void DecreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.HaganahTrack > 1) // Ensure Haganah Track doesn't go below 1
            {
                gameSetup.HaganahTrack--;
                UpdateBoardState();
            }
        }

        private void FullOperationButton_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.FullOperationTaken = true;
            DisableOptionButtons("Full Operation and Special Activity");
            UpdateBoardState("Full Operation and Special Activity");
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.EventTaken = true;
            DisableOptionButtons("Take Event / Block Event");
            UpdateBoardState("Take Event / Block Event");
        }

        private void LimitedOperationButton_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.LimitedOperationTaken = true;
            DisableOptionButtons("Limited Operation");
            UpdateBoardState("Limited Operation");
        }

        private void DisableOptionButtons(string selectedOption)
        {
            fullOperationButton.IsEnabled = selectedOption != "Full Operation and Special Activity";
            eventButton.IsEnabled = selectedOption != "Take Event / Block Event";
            limitedOperationButton.IsEnabled = selectedOption != "Limited Operation";
        }

        public void ExportGameStateToJson(string filePath)
        {
            var intelMarkerInfo = gameSetup.Initiative == "British"
                ? new { count = gameSetup.GetBritishIntelMarkers().Count, values = gameSetup.GetBritishIntelMarkers().Select(m => m.Value).ToList() }
                : new { count = gameSetup.GetBritishIntelMarkers().Count, values = new List<int>() };

            var gameState = new
            {
                currentPlayer = gameSetup.Initiative,
                initiative = gameSetup.Initiative,
                politicalWill = gameSetup.PoliticalWill,
                haganahTrack = gameSetup.HaganahTrack,
                selectedCapabilities = new
                {
                    british = gameSetup.GetBritishCapabilities().Select(c => new
                    {
                        name = c.Name,
                        selectedOption = GetSelectedOptionText(c)
                    }).ToList(),
                    irgun = gameSetup.GetIrgunCapabilities().Select(c => new
                    {
                        name = c.Name,
                        selectedOption = GetSelectedOptionText(c)
                    }).ToList()
                },
                areas = gameSetup.GetAreas().Select(a => new
                {
                    name = a.Name,
                    type = a.Type,
                    pointValue = a.PointValue,
                    irgunUnits = new
                    {
                        cells = a.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun"),
                        hiddenCells = a.Units.Count(u => u.Type == "Cell" && u.Faction == "Irgun" && u.State == "Hidden"),
                        weapons = a.Units.Count(u => u.Type == "Weapon" && u.Faction == "Irgun")
                    },
                    britishUnits = new
                    {
                        police = a.Units.Count(u => u.Type == "Police" && u.Faction == "British"),
                        troops = a.Units.Count(u => u.Type == "Troop" && u.Faction == "British")
                    },
                    markers = a.Markers.Select(m => new { type = m.Type, effect = m.Effect }).ToList() // Added markers info
                }).ToList(),
                eventDeck = new
                {
                    drawPileCount = gameSetup.GetDrawPile().Count,
                    discardPileCount = gameSetup.GetDiscardPile().Count
                },
                currentCard = new
                {
                    title = currentCard.Title,
                    type = currentCard.Type
                },
                availableOptions = GetAvailableOptions(),
                intelMarkerInfo
            };

            var json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }








        private string GetSelectedOptionText(Capability capability)
        {
            return capability.SelectedOption switch
            {
                1 => capability.Option1,
                2 => capability.Option2,
                _ => "No option selected"
            };
        }

        private List<string> GetAvailableOptions()
        {
            var availableOptions = new List<string>();

            if (!gameSetup.FullOperationTaken)
            {
                availableOptions.Add("Full Operation and Special Activity");
            }
            if (!gameSetup.EventTaken)
            {
                availableOptions.Add("Take Event / Block Event");
            }
            if (!gameSetup.LimitedOperationTaken)
            {
                availableOptions.Add("Limited Operation");
            }

            return availableOptions;
        }





        private void ExportGameStateButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "gameState.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportGameStateToJson(saveFileDialog.FileName);
            }
        }
        private void OpenManageMarkersWindow_Click(object sender, RoutedEventArgs e)
        {
            ManageMarkersWindow manageMarkersWindow = new ManageMarkersWindow(gameSetup);
            manageMarkersWindow.Owner = this;
            manageMarkersWindow.ShowDialog();
            UpdateBoardState();
        }




    }
}
