using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TBW
{
    public partial class MainWindow : Window
    {
        private GameSetup gameSetup;

        public MainWindow()
        {
            InitializeComponent();
            gameSetup = new GameSetup();
            gameSetup.SetupDeck();
            UpdateBoardState();
        }

        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            var card = gameSetup.DrawCard();
            if (card != null)
            {
                cardTitleTextBlock.Text = card.Name;
                cardOptionsTextBox.Text = $"Option 1: {card.Option1}\nOption 2: {card.Option2}";

                if (card.Type == "Capability")
                {
                    capabilityTextBlock.Text = $"{card.Name}\nOption 1: {card.Option1}\nOption 2: {card.Option2}";
                    selectBritishCapabilityButton.IsEnabled = true;
                    selectIrgunCapabilityButton.IsEnabled = true;
                }
                else
                {
                    capabilityTextBlock.Text = "No capability selected.";
                    selectBritishCapabilityButton.IsEnabled = false;
                    selectIrgunCapabilityButton.IsEnabled = false;
                }
                UpdateDeckCounts();
            }
        }


        private void SelectOption1_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = gameSetup.GetDiscardPile().LastOrDefault();
            if (currentCard != null)
            {
                MessageBox.Show($"Option 1 selected for {currentCard.Name}: {currentCard.Option1}");
                // Perform actions related to Option 1
            }
        }

        private void SelectOption2_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = gameSetup.GetDiscardPile().LastOrDefault();
            if (currentCard != null)
            {
                MessageBox.Show($"Option 2 selected for {currentCard.Name}: {currentCard.Option2}");
                // Perform actions related to Option 2
            }
        }



        private void SelectBritishCapabilityButton_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = gameSetup.GetDiscardPile().LastOrDefault();
            if (currentCard != null && currentCard.Type == "Capability")
            {
                var capability = new Capability(currentCard.Name, currentCard.Option1, currentCard.Option2);
                gameSetup.AddBritishCapability(capability);
                UpdateBoardState();
            }
        }

        private void SelectIrgunCapabilityButton_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = gameSetup.GetDiscardPile().LastOrDefault();
            if (currentCard != null && currentCard.Type == "Capability")
            {
                var capability = new Capability(currentCard.Name, currentCard.Option1, currentCard.Option2);
                gameSetup.AddIrgunCapability(capability);
                UpdateBoardState();
            }
        }

        private void UpdateDeckCounts()
        {
            if (drawDeckTextBlock != null)
            {
                drawDeckTextBlock.Text = $"Draw Deck: {gameSetup.GetDrawPile().Count} cards";
            }

            if (discardDeckTextBlock != null)
            {
                discardDeckTextBlock.Text = $"Discard Pile: {gameSetup.GetDiscardPile().Count} cards";
            }
        }

        private void UpdateBoardState()
        {
            var britishCapabilities = gameSetup.GetBritishCapabilities();
            if (britishCapabilities.Any())
            {
                var capability = britishCapabilities.Last();
                britishCapabilityTextBlock.Text = $"{capability.Name}\nOption 1: {capability.Option1}\nOption 2: {capability.Option2}";
            }
            else
            {
                britishCapabilityTextBlock.Text = "No capability selected.";
            }

            var irgunCapabilities = gameSetup.GetIrgunCapabilities();
            if (irgunCapabilities.Any())
            {
                var capability = irgunCapabilities.Last();
                irgunCapabilityTextBlock.Text = $"{capability.Name}\nOption 1: {capability.Option1}\nOption 2: {capability.Option2}";
            }
            else
            {
                irgunCapabilityTextBlock.Text = "No capability selected.";
            }

            // Update board state
            boardTextBox.Text = GetBoardStateText();
        }

        private string GetBoardStateText()
        {
            var areas = gameSetup.GetAreas();
            var boardState = "Board State:\n";
            foreach (var area in areas)
            {
                boardState += $"{area.Name} ({area.Type}): {area.PointValue} points\n";
                foreach (var adjacentArea in area.AdjacentAreas)
                {
                    boardState += $"  Adjacent to: {adjacentArea}\n";
                }
                foreach (var unit in area.Units)
                {
                    boardState += $"  Unit: {unit.Type}, Status: {unit.Status}\n";
                }
            }
            return boardState;
        }

        private void InitiativeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (initiativeToggleButton != null && gameSetup != null)
            {
                initiativeToggleButton.Content = "Initiative: British";
                gameSetup.Initiative = "British";
            }
        }

        private void InitiativeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (initiativeToggleButton != null && gameSetup != null)
            {
                initiativeToggleButton.Content = "Initiative: Irgun";
                gameSetup.Initiative = "Irgun";
            }
        }
    }
}
