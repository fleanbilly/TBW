using Newtonsoft.Json;
using System.Linq;
using System.Windows;

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
            DisplayNextCard();
            UpdateBoardState();
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
                UpdateDeckCounts();
                UpdateBoardState();
            }
            else
            {
                MessageBox.Show("No more cards in the draw pile.");
            }
        }
        private void UpdateDeckCounts()
        {
            drawDeckTextBlock.Text = $"Draw Deck: {gameSetup.GetDrawPile().Count} cards";
            discardDeckTextBlock.Text = $"Discard Pile: {gameSetup.GetDiscardPile().Count} cards";
        }


        private void UpdateBoardState()
        {
            boardTextBox.Text = "Board State and Scoring Information\n";
            boardTextBox.Text += $"Current Player: {gameSetup.Initiative}\n";
            boardTextBox.Text += $"Political Will: {gameSetup.PoliticalWill}\n";
            boardTextBox.Text += $"Haganah Track: {gameSetup.HaganahTrack}\n\n";

            boardTextBox.Text += "Selected Capabilities:\n";
            var selectedBritishCapabilities = gameSetup.GetBritishCapabilities();
            var selectedIrgunCapabilities = gameSetup.GetIrgunCapabilities();

            foreach (var capability in selectedBritishCapabilities)
            {
                boardTextBox.Text += $"British: {capability.Name} - {GetSelectedOptionText(capability)}\n";
            }

            foreach (var capability in selectedIrgunCapabilities)
            {
                boardTextBox.Text += $"Irgun: {capability.Name} - {GetSelectedOptionText(capability)}\n";
            }

            boardTextBox.Text += "\nAreas and Units:\n";
            foreach (var area in gameSetup.GetAreas())
            {
                boardTextBox.Text += $"{area.Name} ({area.Type}) - Points: {area.PointValue}, Adjacent: {string.Join(", ", area.AdjacentAreas)}\n";
                foreach (var unit in area.Units)
                {
                    boardTextBox.Text += $"- {unit.Type} Unit, State: {unit.State}\n";
                }
                boardTextBox.Text += "\n";
            }

            initiativeToggleButton.Content = gameSetup.Initiative;
            politicalWillTextBlock.Text = gameSetup.PoliticalWill.ToString();
            haganahTrackTextBlock.Text = gameSetup.HaganahTrack.ToString();
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

        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayNextCard();
        }

        private void ShowDiscardPileButton_Click(object sender, RoutedEventArgs e)
        {
            var discardPile = gameSetup.GetDiscardPile();
            string discardPileText = "Discard Pile:\n" + string.Join("\n", discardPile.Select(c => c.Title));
            MessageBox.Show(discardPileText, $"Discard Pile ({discardPile.Count} cards played)");
            UpdateDeckCounts(); // Ensure the counts are updated
        }

        private void ShowDrawPileButton_Click(object sender, RoutedEventArgs e)
        {
            var drawPile = gameSetup.GetDrawPile();
            MessageBox.Show($"There are {drawPile.Count} cards left to play.", $"Draw Pile ({drawPile.Count} cards left to play)");
            UpdateDeckCounts(); // Ensure the counts are updated
        }

        private void SelectOption1Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCard != null)
            {
                currentCard.SelectOption(1);
                cardSelectedOptionTextBlock.Text = "Selected Option: 1";
                UpdateBoardStateWithSelectedOption(currentCard, currentCard.Option1);
            }
        }

        private void SelectOption2Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentCard != null)
            {
                currentCard.SelectOption(2);
                cardSelectedOptionTextBlock.Text = "Selected Option: 2";
                UpdateBoardStateWithSelectedOption(currentCard, currentCard.Option2);
            }
        }

        private void UpdateBoardStateWithSelectedOption(Card card, string selectedOption)
        {
            boardTextBox.Text = $"Selected option for {card.Title}: {selectedOption}";
            // If it's a capability card, add it to the relevant capabilities list
            if (card.Type == "Capability")
            {
                var capability = new Capability(card.Title, card.Option1, card.Option2);
                capability.SelectOption(card.SelectedOption);
                if (gameSetup.Initiative == "British")
                {
                    gameSetup.AddBritishCapability(capability);
                }
                else if (gameSetup.Initiative == "Irgun")
                {
                    gameSetup.AddIrgunCapability(capability);
                }
            }
            UpdateBoardState();
        }

        private void InitiativeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "Irgun";
                initiativeToggleButton.Content = "Irgun";
                UpdateBoardState();
            }
        }

        private void InitiativeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (gameSetup != null)
            {
                gameSetup.Initiative = "British";
                initiativeToggleButton.Content = "British";
                UpdateBoardState();
            }
        }

        private void IncreasePoliticalWill_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.PoliticalWill++;
            politicalWillTextBlock.Text = gameSetup.PoliticalWill.ToString();
        }

        private void DecreasePoliticalWill_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.PoliticalWill > 0) // Ensure Political Will doesn't go below 0
            {
                gameSetup.PoliticalWill--;
                politicalWillTextBlock.Text = gameSetup.PoliticalWill.ToString();
            }
        }

        private void IncreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            gameSetup.HaganahTrack++;
            haganahTrackTextBlock.Text = gameSetup.HaganahTrack.ToString();
        }

        private void DecreaseHaganahTrack_Click(object sender, RoutedEventArgs e)
        {
            if (gameSetup.HaganahTrack > 0) // Ensure Haganah Track doesn't go below 0
            {
                gameSetup.HaganahTrack--;
                haganahTrackTextBlock.Text = gameSetup.HaganahTrack.ToString();
            }
        }
    }
}
