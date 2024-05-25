using Newtonsoft.Json;
using System.Linq;
using System.Windows;

namespace TBW
{
    public partial class MainWindow : Window
    {
        private GameSetup gameSetup;

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
            Card nextCard = gameSetup.DrawCard();
            if (nextCard != null)
            {
                cardTitleTextBlock.Text = nextCard.Title;
                cardTextTextBlock.Text = nextCard.Text;
                UpdateBoardState();
            }
            else
            {
                MessageBox.Show("No more cards in the draw pile.");
            }
        }

        private void UpdateBoardState()
        {
            // Create a textual representation of the map and unit placement
            boardTextBox.Text = "Board State and Scoring Information\n";
            boardTextBox.Text += $"Current Player: {gameSetup.Initiative}\n";
            boardTextBox.Text += $"Political Will: {gameSetup.PoliticalWill}\n";
            boardTextBox.Text += $"Haganah Track: {gameSetup.HaganahTrack}\n\n";

            foreach (var area in gameSetup.GetAreas())
            {
                boardTextBox.Text += $"{area.Name} (Points: {area.PointValue}) - Adjacent: {string.Join(", ", area.AdjacentAreas)}\n";
                foreach (var unit in area.Units)
                {
                    boardTextBox.Text += $"- {unit.Type} Unit, State: {unit.State}\n";
                }
                boardTextBox.Text += "\n";
            }

            // Update capabilities
            britishCapabilitiesTextBox.Text = string.Join("\n", gameSetup.GetBritishCapabilities().Select(c => $"{c.Name}: {c.Description}"));
            irgunCapabilitiesTextBox.Text = string.Join("\n", gameSetup.GetIrgunCapabilities().Select(c => $"{c.Name}: {c.Description}"));

            // Update game status
            initiativeTextBlock.Text = $"Initiative: {gameSetup.Initiative}";
            politicalWillTextBlock.Text = $"Political Will: {gameSetup.PoliticalWill}";
            haganahTrackTextBlock.Text = $"Haganah Track: {gameSetup.HaganahTrack}";
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
    }
}
