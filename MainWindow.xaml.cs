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
       
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("game_log.txt"))
            {
                File.SetAttributes("game_log.txt", FileAttributes.Normal);
                File.Delete("game_log.txt");
            }
            // Create a new game log file
            File.Create("game_log.txt").Dispose();
            LogInitialGameState(); // Log the initial game state
            DrawNextCard();
            UpdateBoardState();
            DrawCardButton.IsEnabled = true;
            selectOption1Button.IsEnabled = false; // Disable initially
            selectOption2Button.IsEnabled = false; // Disable initially
        }
        private void SaveGameLogButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                FileName = "game_log.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy("game_log.txt", saveFileDialog.FileName, true);
                MessageBox.Show("Game log saved successfully.", "Save Game Log", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DrawNextCard()
        {
            currentCard = gameSetup.DrawCard();
            if (currentCard != null)
            {
                DisplayCurrentCard();
              
            }
            else
            {
                MessageBox.Show("No more cards in the draw pile.");
            }
        }




        //private void DrawInitialCard()
        //{
        //    currentCard = gameSetup.PeekAtTopCard(); // Peek at the top card without removing it from the deck
        //    if (currentCard != null)
        //    {
        //        DisplayCurrentCard();
        //    }
        //    else
        //    {
        //        MessageBox.Show("No more cards in the draw pile.");
        //    }
        //}


        private void DisplayCurrentCard()
        {
            cardTitleTextBlock.Text = currentCard.Title;
            cardOption1TextBlock.Text = currentCard.Option1;
            cardOption2TextBlock.Text = currentCard.Option2;
            cardSelectedOptionTextBlock.Text = "Selected Option: None";
            selectOption1Button.IsEnabled = true;
            selectOption2Button.IsEnabled = true;
            LogGameAction("New Draw Card", $"Card {gameSetup.TurnNumber} drawn.\n{currentCard.Option1}\n{currentCard.Option2}");
        }

        // Add this method to handle logging the event action
        private void LogCapabilityAction(string title, string option, string faction)
        {
            string logEntry = $"{DateTime.Now}: {faction} selected capability '{title}' with '{option}'.";
            LogGameAction("Capability Selected", logEntry);
        }

        private void LogEventAction(string title, string option)
        {
            string logEntry = $"{gameSetup.CurrentTurnPlayer} played an Event Card: Event '{title}' executed option '{option}'.";
            LogGameAction("Event Card Played", logEntry);
          
        }

        private void LogPropagandaAction(string title)
        {
            string logEntry = $"{DateTime.Now}: Propaganda card '{title}' executed.";
            File.AppendAllText("game_log.txt", logEntry + Environment.NewLine);
            gameLogTextBox.AppendText(logEntry + Environment.NewLine);
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
            LogGameAction("Draw Intel Marker", "British drew Intel Marker");
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



        public void UpdateBoardState(string selectedOption = "")
        {
            boardTextBox.Text = "Board State and Scoring Information\n";
            boardTextBox.Text += $"Current Player: {gameSetup.Initiative}\n";
            boardTextBox.Text += $"Political Will: {gameSetup.PoliticalWill}\n";
            boardTextBox.Text += $"Haganah Track: {gameSetup.HaganahTrack}\n\n";
            boardTextBox.Text += $"Available Options for player: {string.Join(", ", gameSetup.AvailableOptions("list"))}\n\n";
            

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
            
            gameSetup.IncrementTurnNumber();
            DrawNextCard();
            LogGameAction("Draw Card", $"Card {gameSetup.TurnNumber} drawn.");
            UpdateBoardState();
            fullOperationButton.IsEnabled = true;
            limitedOperationButton.IsEnabled = true;
            eventButton.IsEnabled = true;
            gameSetup.AvailableOptions("reset");


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
                        LogCapabilityAction(currentCard.Title, currentCard.Option1, "British");
                        UpdateBoardState();
                    }
                }
                else if (currentCard.Type == "Event")
                {
                    LogEventAction(currentCard.Title, currentCard.Option1);
                    UpdateBoardState();
                }
                else if (currentCard.Type == "Propaganda")
                {
                    LogPropagandaAction(currentCard.Title);
                    UpdateBoardState();
                }

               
            }
            eventButton.IsEnabled = false;
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
                        gameSetup.AddIrgunCapability(capability); // Add to the selected list
                        LogCapabilityAction(currentCard.Title, currentCard.Option2, "Irgun");
                        UpdateBoardState();
                    }
                }
                else if (currentCard.Type == "Event")
                {
                    LogEventAction(currentCard.Title, currentCard.Option2);
                    UpdateBoardState();
                }
                else if (currentCard.Type == "Propaganda")
                {
                    LogPropagandaAction(currentCard.Title);
                    UpdateBoardState();
                }

                
            }
            eventButton.IsEnabled = false;
        }


        private void FullOperationButton_Click(object sender, RoutedEventArgs e)
        {
            FullOperationWindow fullOperationWindow = new FullOperationWindow(gameSetup, this);
           
            fullOperationWindow.Owner = this;

            if (fullOperationWindow.ShowDialog() == true)
            {
                string loggedActions = fullOperationWindow.GetLoggedActions();
                LogGameAction($"{gameSetup.CurrentTurnPlayer} Took a Full Operation and/or a Special Activity", loggedActions);
                fullOperationButton.IsEnabled = false;

            }
            else {
                fullOperationButton.IsEnabled = true;
            }
            gameSetup.AvailableOptions("remove", "Full Operation and Special Activity");
        }



        private void InitiativeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "Irgun";
                initiativeToggleButton.Content = "Initiative: Irgun";
                gameSetup.CurrentTurnPlayer = "Irgun";
                UpdateBoardState();
            }
        }

        private void InitiativeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "British";
                gameSetup.CurrentTurnPlayer = "British";
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
                LogGameAction($"{gameSetup.CurrentTurnPlayer} -Increase Political Will", $"Political Will increased to {gameSetup.PoliticalWill}");
            }
        }

        private void DecreasePoliticalWill_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.PoliticalWill > 0) // Ensure Political Will doesn't go below 0
            {
                gameSetup.PoliticalWill--;
                UpdateBoardState();
                LogGameAction($"{gameSetup.CurrentTurnPlayer} -Decrease Political Will", $"Political Will decreased to {gameSetup.PoliticalWill}");
            }
        }

        private void IncreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.HaganahTrack < 4)
            {
                gameSetup.HaganahTrack++;
                UpdateBoardState();
                LogGameAction($"{gameSetup.CurrentTurnPlayer} -Increase Haganah Track", $"Haganah Track increased to {gameSetup.HaganahTrack}");
            }
        }

        private void DecreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.HaganahTrack > 1) // Ensure Haganah Track doesn't go below 1
            {
                gameSetup.HaganahTrack--;
                UpdateBoardState();
                LogGameAction($"{gameSetup.CurrentTurnPlayer} -Decrease Haganah Track", $"Haganah Track decreased to {gameSetup.HaganahTrack}");
            }
        }

       private void LogBoardState_Click(object sender, RoutedEventArgs e)
        {
            LogGameAction($"Board State after {gameSetup.CurrentTurnPlayer} moves.", ExportGameStateToJson("game_state.json"));
           
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.EventTaken = true;
            DisableOptionButtons("Take Event / Block Event");
            selectOption1Button.IsEnabled = true; // Enable when event action is chosen
            selectOption2Button.IsEnabled = true; // Enable when event action is chosen
            gameSetup.AvailableOptions("remove", "Take Event / Block Event");
            UpdateBoardState("Take Event / Block Event");
        }


        private void LimitedOperationButton_Click(object sender, RoutedEventArgs e)
        {
            var limitedOperationWindow = new LimitedOperationWindow(gameSetup, this);
            limitedOperationWindow.Owner = this;

            if (limitedOperationWindow.ShowDialog() == true)
            {
                string selectedOperation = limitedOperationWindow.SelectedOperation;
                string selectedArea = limitedOperationWindow.SelectedArea;

                if (!string.IsNullOrEmpty(selectedOperation) && !string.IsNullOrEmpty(selectedArea))
                {
                    LogGameAction($"{gameSetup.CurrentTurnPlayer} performed {selectedOperation} in {selectedArea}", "");
                    if (gameSetup.HaganahTrack < 4)
                    {
                        limitedOperationButton.IsEnabled = false;
                    }

                }
            }
            else { 
             limitedOperationButton.IsEnabled = true;
            }
            gameSetup.AvailableOptions("remove", "Limited Operation");
        }




        private void DisableOptionButtons(string selectedOption)
        {
            fullOperationButton.IsEnabled = selectedOption != "Full Operation and Special Activity";
            eventButton.IsEnabled = selectedOption != "Take Event / Block Event";
            limitedOperationButton.IsEnabled = selectedOption != "Limited Operation";
        }

        public string ExportGameStateToJson(string filePath, bool exportJson = false)
        {
            // Ensure currentCard is not null by peeking at the top card
            if (currentCard == null)
            {
                currentCard = gameSetup.PeekAtTopCard();
            }

            var intelMarkerInfo = gameSetup.Initiative == "British"
                ? new { count = gameSetup.GetBritishIntelMarkers().Count, values = gameSetup.GetBritishIntelMarkers().Select(m => m.Value).ToList() }
                : new { count = gameSetup.GetBritishIntelMarkers().Count, values = new List<int>() };

            var gameState = new
            {
                Tips = "AI should remember and  always consider these points before declaring move.\n" +
                "Conduct operations in the maximum numbers of spaces a turn choice allows.\n " +
                "Full Operations allow up to 3 spaces for an operation type if conditions are met. If the Haganah track is at 4 then 4 spaces can be selected.  (2 for limited operation)\n" +
                "Does the operations space selected get a bonus from using (removing) a weapons cache. Some operations allow the use of weapons cache from adjacent areas. Utilize weapons caches for operations in higher value areas.\n" +
                "Always ensure the operational space selected meets the requirements to conduct that operation in that space (under location).\n",
                turnNumber = gameSetup.TurnNumber,
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
                    adjacentAreas = a.AdjacentAreas,
                    coastal = a.isCoastal,
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
                    markers = a.Markers.Select(m => new { type = m.Type, effect = m.Effect }).ToList()
                }).ToList(),
                eventDeck = new
                {
                    drawPileCount = gameSetup.GetDrawPile().Count,
                    discardPileCount = gameSetup.GetDiscardPile().Count
                },
                currentCard = currentCard != null
                    ? new { title = currentCard.Title, type = currentCard.Type, option1 = currentCard.Option1, option2 = currentCard.Option2 }
                    : new { title = "No Card", type = "None", option1 = "", option2 = "" },
                availableOptions = GetAvailableOptions(),
                intelMarkerInfo
            };

            var json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            if (exportJson)
            {
                string directory = Path.GetDirectoryName(filePath);
                string filename = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string newFilePath = Path.Combine(directory, gameSetup.TurnNumber.ToString() + '_' + filename + extension);
                File.WriteAllText(newFilePath, json);
            }

            return json; // Return the file path where the JSON was saved - not used
        }



        private void LogInitialGameState()
        {
            string initialState = ExportGameStateToJson("game_state_initial.json");
            LogGameAction("Initial Game State", initialState);
           
        }

        public void LogGameAction(string actionDescription, string actionDetails)
        {
            if (!File.Exists("game_log.txt"))
            {
                // Create or overwrite the game log file
                using (StreamWriter writer = new StreamWriter("game_log.txt", false))
                {
                    writer.WriteLine("Game Log reCreated: " + DateTime.Now);
                }
            }

            string logEntry = $"Game Event: {actionDescription}\n{actionDetails}\n";
            File.AppendAllText("game_log.txt", logEntry);
            gameLogTextBox.AppendText(logEntry + Environment.NewLine);
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

        private Array GetAvailableOptions()
        {

            return gameSetup.AvailableOptions("list");  
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
                ExportGameStateToJson(saveFileDialog.FileName,true);
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
