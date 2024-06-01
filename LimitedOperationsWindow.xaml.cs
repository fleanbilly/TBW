using System.Windows;

namespace TBW
{
    public partial class LimitedOperationWindow : Window
    {
        private GameSetup gameSetup;
        private MainWindow mainWindow;

        public string SelectedOperation { get; private set; }
        public string SelectedArea { get; private set; }

        public LimitedOperationWindow(GameSetup setup, MainWindow main)
        {
            InitializeComponent();
            gameSetup = setup;
            mainWindow = main;
            PopulateOperationsComboBox();
            PopulateAreaComboBox();
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

        private void PopulateAreaComboBox()
        {
            areaComboBox.Items.Clear();
            foreach (var area in gameSetup.GetAreas())
            {
                areaComboBox.Items.Add(area.Name);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedOperation = operationComboBox.SelectedItem?.ToString();
            SelectedArea = areaComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(SelectedOperation) || string.IsNullOrEmpty(SelectedArea))
            {
                MessageBox.Show("Please select both an operation and an area.");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ManageUnitsButton_Click(object sender, RoutedEventArgs e)
        {
            ManageUnitsWindow manageUnitsWindow = new ManageUnitsWindow(gameSetup, mainWindow);
            manageUnitsWindow.Owner = this;
            manageUnitsWindow.ShowDialog();
            PopulateAreaComboBox(); // Refresh the area combo box after managing units
        }
    }
}
