using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TBW
{
    public partial class ManageUnitsWindow : Window
    {
        private GameSetup gameSetup;
        private MainWindow mainWindow;

        public ManageUnitsWindow(GameSetup setup, MainWindow main)
        {
            InitializeComponent();
            gameSetup = setup;
            mainWindow = main;

            // Populate areaComboBox and toAreaComboBox
            var areas = gameSetup.GetAreas().Select(a => a.Name).ToList();
            areaComboBox.ItemsSource = areas;
            toAreaComboBox.ItemsSource = areas;

            // Populate unitTypeComboBox
            unitTypeComboBox.ItemsSource = new[] { "Cell", "Weapons", "Troop", "Police" };

            // Populate factionComboBox
            factionComboBox.ItemsSource = new[] { "Irgun", "British" };

            // Populate stateComboBox
            stateComboBox.ItemsSource = new[] { "Hidden", "Active" };

            unitTypeComboBox.SelectionChanged += unitTypeComboBox_SelectionChanged;
            factionComboBox.SelectionChanged += factionComboBox_SelectionChanged;

            ToggleStateButtonState(); // Initial button state check
        }

        private void AreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUnitsList();
        }

        private void AddUnitButton_Click(object sender, RoutedEventArgs e)
        {
            string areaName = areaComboBox.SelectedItem.ToString();
            string unitType = unitTypeComboBox.SelectedItem.ToString();
            string faction = factionComboBox.SelectedItem.ToString();
            string state = stateComboBox.SelectedItem.ToString(); // Change here

            Unit unit = new Unit(unitType, faction, state);
            gameSetup.AddUnitToArea(areaName, unit);
            UpdateUnitsList();
            MessageBox.Show($"Added {unit}");
            mainWindow.RefreshBoardState(); // Refresh the board state in MainWindow
        }

        private void RemoveUnitButton_Click(object sender, RoutedEventArgs e)
        {
            string areaName = areaComboBox.SelectedItem.ToString();
            string unitType = unitTypeComboBox.SelectedItem.ToString();
            string faction = factionComboBox.SelectedItem.ToString();
            string state = stateComboBox.SelectedItem.ToString(); // Change here

            Unit unit = new Unit(unitType, faction, state);
            gameSetup.RemoveUnitFromArea(areaName, unit);
            UpdateUnitsList();
            MessageBox.Show($"Removed {unit}");
            mainWindow.RefreshBoardState(); // Refresh the board state in MainWindow
        }


        private void MoveUnitButton_Click(object sender, RoutedEventArgs e)
        {
            string fromAreaName = areaComboBox.SelectedItem.ToString();
            string toAreaName = toAreaComboBox.SelectedItem.ToString();
            string unitType = unitTypeComboBox.SelectedItem.ToString();
            string faction = factionComboBox.SelectedItem.ToString();
            string state = stateComboBox.SelectedItem.ToString();

            Unit unit = new Unit(unitType, faction, state);

            var fromArea = gameSetup.GetAreaByName(fromAreaName);
            var toArea = gameSetup.GetAreaByName(toAreaName);

            if (fromArea != null && toArea != null)
            {
                var unitToMove = fromArea.Units.FirstOrDefault(u => u.Equals(unit));
                if (unitToMove != null)
                {
                    fromArea.RemoveUnit(unitToMove);
                    toArea.AddUnit(unitToMove);
                    UpdateUnitsList();
                    MessageBox.Show($"Moved {unit} from {fromAreaName} to {toAreaName}");
                    mainWindow.RefreshBoardState(); // Refresh the board state in MainWindow
                }
                else
                {
                    MessageBox.Show("The specified unit does not exist in the selected area.");
                }
            }
        }



        private void UnitsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (unitsListBox.SelectedItem is string selectedUnitString)
            {
                // Expected format: "Type (Faction, State)"
                var parts = selectedUnitString.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    unitTypeComboBox.SelectedItem = parts[0].Trim();
                    factionComboBox.SelectedItem = parts[1].Trim();
                    stateComboBox.SelectedItem = parts[2].Trim();
                }
            }
        }

        private void UpdateUnitsList()
        {
            string selectedAreaName = areaComboBox.SelectedItem?.ToString();
            var area = gameSetup.GetAreaByName(selectedAreaName);
            if (area != null)
            {
                unitsListBox.ItemsSource = area.Units.Select(u => $"{u.Type} ({u.Faction}, {u.State})");
            }
            else
            {
                unitsListBox.ItemsSource = null;
            }
        }

        private void ToggleStateButton_Click(object sender, RoutedEventArgs e)
        {
            string areaName = areaComboBox.SelectedItem.ToString();
            string unitType = unitTypeComboBox.SelectedItem.ToString();
            string faction = factionComboBox.SelectedItem.ToString();

            if (unitType == "Cell" && faction == "Irgun")
            {
                var area = gameSetup.GetAreaByName(areaName);
                if (area != null)
                {
                    var unitToToggle = area.Units.FirstOrDefault(u => u.Type == unitType && u.Faction == faction);
                    if (unitToToggle != null)
                    {
                        unitToToggle.State = unitToToggle.State == "Hidden" ? "Active" : "Hidden";
                        UpdateUnitsList();
                        MessageBox.Show($"Toggled state of {unitType} ({faction}) in {areaName} to {unitToToggle.State}");
                        mainWindow.RefreshBoardState(); // Refresh the board state in MainWindow
                    }
                    else
                    {
                        MessageBox.Show("The specified unit does not exist in the selected area.");
                    }
                }
            }
        }
        private void unitTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToggleStateButtonState();
        }

        private void factionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToggleStateButtonState();
        }

        private void ToggleStateButtonState()
        {
            string unitType = unitTypeComboBox.SelectedItem?.ToString();
            string faction = factionComboBox.SelectedItem?.ToString();

            if (unitType == "Cell" && faction == "Irgun")
            {
                toggleStateButton.IsEnabled = true;
            }
            else
            {
                toggleStateButton.IsEnabled = false;
            }
        }


    }
}
