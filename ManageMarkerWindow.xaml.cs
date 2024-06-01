using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TBW
{
    public partial class ManageMarkersWindow : Window
    {
        private GameSetup gameSetup;

        public ManageMarkersWindow(GameSetup setup)
        {
            InitializeComponent();
            gameSetup = setup;

            // Populate areaComboBox
            var areas = gameSetup.GetAreas().Select(a => a.Name).ToList();
            areaComboBox.ItemsSource = areas;

            UpdateMarkersListBox();
        }

        private void areaComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateMarkersListBox();
        }

        private void AddMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (areaComboBox.SelectedItem != null && markerTypeComboBox.SelectedItem != null)
            {
                var selectedArea = areaComboBox.SelectedItem.ToString();
                var selectedMarker = ((System.Windows.Controls.ComboBoxItem)markerTypeComboBox.SelectedItem).Content.ToString();
                var area = gameSetup.GetAreaByName(selectedArea);
                area.AddMarker(selectedMarker);
                UpdateMarkersListBox();
                RefreshBoardState();
                AddMarkerToLog(selectedMarker,selectedArea);
            }
        }

        private void RemoveMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (areaComboBox.SelectedItem != null && markersListBox.SelectedItem != null)
            {
                var selectedArea = areaComboBox.SelectedItem.ToString();
                var selectedMarker = markersListBox.SelectedItem.ToString();
                var area = gameSetup.GetAreaByName(selectedArea);
                area.RemoveMarker(selectedMarker);
                UpdateMarkersListBox();
                RefreshBoardState();
            }
        }

        private void markersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (markersListBox.SelectedItem != null)
            {
                markerTypeComboBox.SelectedItem = markersListBox.SelectedItem.ToString();
            }
        }


        private void UpdateMarkersListBox()
        {
            if (areaComboBox.SelectedItem != null)
            {
                var selectedArea = areaComboBox.SelectedItem.ToString();
                var area = gameSetup.GetAreaByName(selectedArea);
                markersListBox.ItemsSource = area.Markers.Select(m => m.Type).ToList();
            }
        }


        private void RefreshBoardState()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.RefreshBoardState();
            }
        }
        private void AddMarkerToLog(string marker, string area)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.LogGameAction("Marker Added",$"{gameSetup.CurrentTurnPlayer} added {marker} to {area}");
            }
        }
    }
}
