using System;
using System.Linq;
using System.Windows;

namespace TBW
{
    public partial class FullOperationWindow : Window
    {
        private GameSetup gameSetup;
        private MainWindow mainWindow;
        private string actionLog;

        public FullOperationWindow(GameSetup setup, MainWindow main)
        {
            InitializeComponent();
            gameSetup = setup;
            mainWindow = main;
            PopulateOperationsComboBox();
            PopulateAreasComboBox();
            PopulateSpecialActivityComboBox();
            actionLog = string.Empty;
        }

        private void PopulateOperationsComboBox()
        {
            operationComboBox.Items.Clear();
            if (gameSetup.CurrentTurnPlayer == "British")
            {
                operationComboBox.Items.Add("Deploy");
                operationComboBox.Items.Add("Patrol");
                operationComboBox.Items.Add("Search");
                operationComboBox.Items.Add("Assault");
            }
            else if (gameSetup.CurrentTurnPlayer == "Irgun")
            {
                operationComboBox.Items.Add("Recruit");
                operationComboBox.Items.Add("Travel");
                operationComboBox.Items.Add("Sabotage");
                operationComboBox.Items.Add("Rob");
            }
        }

        private void PopulateAreasComboBox()
        {
            areaComboBox.Items.Clear();
            foreach (var area in gameSetup.GetAreas())
            {
                areaComboBox.Items.Add(area.Name);
            }
        }

        private void PopulateSpecialActivityComboBox()
        {
            specialActivityComboBox.Items.Clear();
            if (gameSetup.CurrentTurnPlayer == "British")
            {
                specialActivityComboBox.Items.Add("Restore");
                specialActivityComboBox.Items.Add("Negotiate");
                specialActivityComboBox.Items.Add("Mass Detention");
            }
            else if (gameSetup.CurrentTurnPlayer == "Irgun")
            {
                specialActivityComboBox.Items.Add("Terror");
                specialActivityComboBox.Items.Add("Propagandize");
                specialActivityComboBox.Items.Add("Silence");
            }
        }

        private void AddOperationButton_Click(object sender, RoutedEventArgs e)
        {
            string operation = operationComboBox.SelectedItem?.ToString();
            string area = areaComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(area))
            {
                actionLog += $"Operation: {operation}, Area: {area}\n";
                MessageBox.Show("Operation added to log.");
            }
            else
            {
                MessageBox.Show("Please select both an operation and an area.");
            }
        }

        private void AddSpecialActivityButton_Click(object sender, RoutedEventArgs e)
        {
            string specialActivity = specialActivityComboBox.SelectedItem?.ToString();
            string area = areaComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(specialActivity) && !string.IsNullOrEmpty(area))
            {
                actionLog += $"Special Activity: {specialActivity}, Area: {area}\n";
                MessageBox.Show("Special Activity added to log.","Logged");
            }
            else
            {
                MessageBox.Show("Please select both a special activity and an area.");
            }
        }

        private void ManageUnitsButton_Click(object sender, RoutedEventArgs e)
        {
            ManageUnitsWindow manageUnitsWindow = new ManageUnitsWindow(gameSetup, mainWindow);
            manageUnitsWindow.Owner = this;
            manageUnitsWindow.ShowDialog();
            mainWindow.UpdateBoardState(); // Refresh the board state after managing units
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        public string GetLoggedActions()
        {
            return actionLog;
        }
    }
}
